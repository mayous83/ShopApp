using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.ListProducts;

public static class ListProductsEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/products", async (int page, int pageSize, string? search, ProductsDbContext dbContext, HttpContext httpContext) =>
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
            
            var handler = new ListProductsHandler(dbContext);
            var request = new ListProductsRequest(search, page, pageSize);
            var result = await handler.HandleAsync(request, httpContext.RequestAborted);
            return Results.Ok(result);
        });
    }
}