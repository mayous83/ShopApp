using ShopApp.Orders.Service.Database;

namespace ShopApp.Orders.Service.Features.ListOrders;

public static class ListOrdersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/orders", async (int page, int pageSize, OrdersDbContext dbContext, HttpContext httpContext) =>
        {
            // Validate the request
            if (page <= 0 || pageSize <= 0)
            {
                return Results.BadRequest("Page and PageSize must be greater than 0.");
            }
            
            // validate the page size
            if (pageSize > 1000)
            {
                return Results.BadRequest("PageSize must be less than or equal to 1000.");
            }
            
            var handler = new ListOrdersHandler(dbContext);
            var orders = await handler.Handle(new ListOrdersRequest(null, page, pageSize), httpContext.RequestAborted);
            return Results.Ok(orders);
        });
    }
}