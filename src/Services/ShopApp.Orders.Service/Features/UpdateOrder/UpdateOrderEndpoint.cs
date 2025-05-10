using ShopApp.Orders.Service.Database;

namespace ShopApp.Orders.Service.Features.UpdateOrder;

public static class UpdateOrderEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPut("/orders/{id:guid}", async (Guid id, UpdateOrderRequest request, OrdersDbContext dbContext, HttpContext httpContext) =>
        {
            var handler = new UpdateOrderHandler(dbContext);
            request.Id = id;
            var result = await handler.Handle(request, httpContext.RequestAborted);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        });
    }
}