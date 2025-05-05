using FluentValidation;
using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.CreateProduct;

public static class CreateProductEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/products", async (
            CreateProductRequest request,
            IValidator<CreateProductRequest> validator,
            ProductsDbContext db, HttpContext httpContext
        ) => {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var handler = new CreateProductHandler(db);
            var id = await handler.Handle(request, httpContext.RequestAborted);
            return Results.Created($"/products/{id}", new { ProductId = id });
        });
    }
}