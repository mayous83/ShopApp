using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features.GetAddressById;

namespace ShopApp.Addresses.Service.Features.GetAddressById;

public static class GetAddressByIdEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/addresses/{id:guid}", 
            async (Guid id, AddressesDbContext dbContext, HttpContext httpContext) =>
                {
                    var handler = new GetAddressByIdHandler(dbContext);
                    var address = 
                        await handler.Handle(new GetAddressByIdRequest { Id = id }, httpContext.RequestAborted);
                    return address is not null ? Results.Ok(address) : Results.NotFound();
                });
    }
}