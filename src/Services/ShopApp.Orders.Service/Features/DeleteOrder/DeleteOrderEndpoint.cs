using ShopApp.Orders.Service.Database;

namespace ShopApp.Orders.Service.Features.DeleteOrder;

public static class DeleteOrderEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/orders/{id:guid}/delete", 
            async (Guid id, OrdersDbContext dbContext, HttpContext httpContext) =>
            {
                var handler = new DeleteOrderHandler(dbContext);
                var result = await handler.Handle(new DeleteOrderRequest(id), httpContext.RequestAborted);
                return result.Success ? Results.Ok() : Results.NotFound();
            });
    }
}