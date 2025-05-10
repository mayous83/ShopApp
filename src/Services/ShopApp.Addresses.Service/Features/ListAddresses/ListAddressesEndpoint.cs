using Microsoft.AspNetCore.Mvc;
using ShopApp.Addresses.Service.Database;

namespace ShopApp.Addresses.Service.Features.ListAddresses;

public static class ListAddressesEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/addresses", async (int page, int pageSize, string? search, AddressesDbContext dbContext, HttpContext httpContext) =>
        {
            var request = new ListAddressesRequest(search, page, pageSize);
            
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
            
            var handler = new ListAddressesHandler(dbContext);
            var addresses = await handler.Handle(request, httpContext.RequestAborted);
            return Results.Ok(addresses);
        });
    }
}