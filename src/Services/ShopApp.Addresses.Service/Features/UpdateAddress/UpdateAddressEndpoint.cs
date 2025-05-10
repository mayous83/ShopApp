using ShopApp.Addresses.Service.Database;

namespace ShopApp.Addresses.Service.Features.UpdateAddress;

public static class UpdateAddressEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPut("/addresses/{id:guid}", async (Guid id, UpdateAddressRequest request, AddressesDbContext dbContext, HttpContext httpContext) =>
        {
            var handler = new UpdateAddressHandler(dbContext);
            request.Id = id;
            var result = await handler.Handle(request, httpContext.RequestAborted);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        });
    }
}