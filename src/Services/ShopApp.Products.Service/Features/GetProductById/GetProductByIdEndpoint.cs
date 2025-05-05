using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.GetProductById;

public static class GetProductByIdEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/products/{id:guid}", async (
            Guid id,
            ProductsDbContext db,
            HttpContext httpContext
        ) =>
        {
            var handler = new GetProductByIdHandler(db);
            var product = await handler.Handle(id, httpContext.RequestAborted);

            return product.Success
                ? Results.Ok(product)
                : Results.NotFound();
        });
    }
}