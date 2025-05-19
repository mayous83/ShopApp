using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.DeleteUser;

public static class DeleteUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapDelete("/users/{id:guid}", async (Guid id, UsersDbContext dbContext, HttpContext httpContext) =>
        {
            var handler = new DeleteUserHandler(dbContext);
            var result = await handler.Handle(new DeleteUserRequest(id), httpContext.RequestAborted);
            return result.Success ? Results.Ok() : Results.NotFound();
        });
    }
}