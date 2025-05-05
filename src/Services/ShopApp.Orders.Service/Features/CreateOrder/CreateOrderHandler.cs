using System.Text.Json;
using ShopApp.Orders.Service.Database;
using ShopApp.Orders.Service.Database.Migrations;
using ShopApp.Shared.Library;
using ShopApp.Shared.Library.DTOs;
using ShopApp.Shared.Library.Messaging;
using ShopApp.Shared.Library.Messaging.Events;

namespace ShopApp.Orders.Service.Features.CreateOrder;

public class CreateOrderHandler
{
    private readonly OrdersDbContext _dbContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMessageBus _messageBus;
    
    public CreateOrderHandler(OrdersDbContext dbContext, IHttpClientFactory httpClientFactory, IMessageBus messageBus)
    {
        _dbContext = dbContext;
        _httpClientFactory = httpClientFactory;
        _messageBus = messageBus;
    }
    
    public async Task<Result<Guid>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        using var usersServiceClient = _httpClientFactory.CreateClient("UsersService");
        
        // Validate user
        var userResponse = await usersServiceClient.GetAsync($"/users/{request.UserId}", cancellationToken);
        if (!userResponse.IsSuccessStatusCode)
        {
            Result<Guid>.FailureResult("User not found");
        }
        
        using var addressesServiceClient = _httpClientFactory.CreateClient("AddressesService");
        
        // Validate address
        var addressResponse = await addressesServiceClient.GetAsync($"/addresses/{request.AddressId}", cancellationToken);
        if (!addressResponse.IsSuccessStatusCode)
        {
            Result<Guid>.FailureResult("Address not found");
        }
        
        using var productsServiceClient = _httpClientFactory.CreateClient("ProductsService");
        
        // Validate items
        foreach (var item in request.Items)
        {
            var jsonContent = JsonSerializer.Serialize(new {ProductId = item.ProductId, Quantity = item.Quantity });
            var productResponse = await productsServiceClient.PostAsync(
                "/products/reserve/", 
                new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"), 
                cancellationToken);
            if (!productResponse.IsSuccessStatusCode)
            {
                var errorResponse = await productResponse.Content.ReadAsStringAsync(cancellationToken);
                var error = JsonSerializer.Deserialize<Result<bool>>(errorResponse);
                if (error is not null && !string.IsNullOrEmpty(error.Message))
                {
                    return Result<Guid>.FailureResult(error.Message);
                }
                
                return Result<Guid>.FailureResult("Product not found");
            }
            
            var product = await productResponse.Content.ReadFromJsonAsync<ProductResponseDto>(cancellationToken: cancellationToken);
            if (product is null || product.Quantity < item.Quantity)
            {
                Result<Guid>.FailureResult($"Product {item.ProductId} is out of stock");
            }
        }
        
        var order = new Order
        {
            EntityId = Guid.NewGuid(),
            UserId = request.UserId,
            AddressId = request.AddressId,
            Items = request.Items.Select(i => new OrderItem
            {
                EntityId = Guid.NewGuid(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList(),
            CreatedAt = DateTime.UtcNow,
            TotalAmount = request.Items.Sum(i => i.Price * i.Quantity)
        };

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        await _messageBus.PublishAsync("order-created", new OrderCreatedEvent {
            OrderId = order.EntityId,
            UserId  = order.UserId,
            Items   = order.Items.Select(i => new OrderCreatedEvent.OrderItem(i.ProductId, i.Quantity)).ToList()
        }, cancellationToken);

        return Result<Guid>.SuccessResult(order.EntityId);
    }
}