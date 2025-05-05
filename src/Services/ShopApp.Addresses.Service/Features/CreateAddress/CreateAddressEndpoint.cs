using FluentValidation;
using ShopApp.Addresses.Service.Database;

namespace ShopApp.Addresses.Service.Features.CreateAddress;

public static class CreateAddressEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/addresses", async (CreateAddressRequest request, IValidator<CreateAddressRequest> validator,
            AddressesDbContext dbContext, HttpContext httpContext) =>
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var handler = new CreateAddressHandler(dbContext);
            var addressId = await handler.Handle(request, httpContext.RequestAborted);
            return Results.Created($"/addresses/{addressId}", addressId);
        });
    }
}