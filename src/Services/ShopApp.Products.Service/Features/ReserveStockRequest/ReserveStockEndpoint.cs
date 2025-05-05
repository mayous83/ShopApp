using FluentValidation;
using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.ReserveStockRequest;

public static class ReserveStockEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/products/reserve", async (
            ReserveStockRequest request,
            IValidator<ReserveStockRequest> validator,
            ProductsDbContext db,
            HttpContext httpContext) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var handler = new ReserveStockHandler(db);
            var result = await handler.Handle(request, httpContext.RequestAborted);

            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        });
    }
}