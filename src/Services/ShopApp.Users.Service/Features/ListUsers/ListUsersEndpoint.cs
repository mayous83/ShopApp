using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.ListUsers;

public static class ListUsersEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/v1/users", async (int page, int pageSize, string? search, UsersDbContext dbContext, HttpContext httpRequest) =>
        {
            var handler = new ListUsersHandler(dbContext);
            var result = await handler.Handle(new ListUsersRequest(search, page, pageSize), httpRequest.RequestAborted);
            return result.Success ? Results.Ok(result.Data) : Results.NotFound();
        });
    }
}