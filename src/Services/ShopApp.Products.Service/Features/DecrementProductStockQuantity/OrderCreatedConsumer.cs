using ShopApp.Products.Service.Database;
using ShopApp.Shared.Library.Messaging;
using ShopApp.Shared.Library.Messaging.Events;

namespace ShopApp.Products.Service.Features.DecrementProductStockQuantity;

public class OrderCreatedConsumer : IEventConsumer<OrderCreatedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OrderCreatedConsumer(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task Handle(OrderCreatedEvent @event)
    {
        // each call gets its own DbContext
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();

        foreach (var item in @event.Items)
        {
            var p = await db.Products.FindAsync(item.ProductId);
            if (p != null)
            {
                p.StockQuantity    -= item.Quantity;
                p.ReservedStockQuantity -= item.Quantity;
            }
        }

        await db.SaveChangesAsync();
    }
}
