using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.DeleteProduct;

public static class DeleteProductEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ProductsDbContext db, HttpContext httpContext) =>
        {
            var handler = new DeleteProductHandler(db);
            var result = await handler.Handle(id, httpContext.RequestAborted);
            return result.Success
                ? Results.Ok(result.Data)
                : Results.NotFound(result.Message);
        });
    }
}