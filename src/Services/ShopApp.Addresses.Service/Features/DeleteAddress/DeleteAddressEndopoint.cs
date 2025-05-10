using ShopApp.Addresses.Service.Database;

namespace ShopApp.Addresses.Service.Features.DeleteAddress;

public static class DeleteAddressEndopoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("/addresses/{id:guid}", async (Guid id, AddressesDbContext dbContext, HttpContext httpContext) =>
        {
            var handler = new DeleteAddressHandler(dbContext);
            var result = await handler.Handle(new DeleteAddressRequest(id), httpContext.RequestAborted);
            return result.Success ? Results.NoContent() : Results.NotFound(result.Message);
        });
    }
}