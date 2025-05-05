using FluentValidation;
using ShopApp.Orders.Service.Database;
using ShopApp.Shared.Library.Messaging;

namespace ShopApp.Orders.Service.Features.CreateOrder;

public static class CreateOrderEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/orders", async (
            CreateOrderRequest request,
            IValidator<CreateOrderRequest> validator,
            OrdersDbContext db, 
            HttpContext httpContext, 
            IHttpClientFactory httpClientFactory,
            IMessageBus messageBus) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var handler = new CreateOrderHandler(db, httpClientFactory, messageBus);
            var orderId = await handler.Handle(request, httpContext.RequestAborted);

            return Results.Created($"/orders/{orderId}", orderId);
        });
    }
}