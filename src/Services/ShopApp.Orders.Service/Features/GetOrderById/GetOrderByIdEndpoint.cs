using ShopApp.Orders.Service.Database;

namespace ShopApp.Orders.Service.Features.GetOrderById;

public static class GetOrderByIdEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/orders/{id:guid}",
            async (Guid id, OrdersDbContext dbContext, HttpContext httpContext) =>
            {
                var handler = new GetOrderByIdHandler(dbContext);
                var order = await handler.Handle(new GetOrderByIdRequest(id), httpContext.RequestAborted);
                return Results.Ok(order);
            });
    }
}