using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.UpdateUser;

public static class UpdateUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPut("/api/v1/users/{id:guid}", async (Guid id, UpdateUserRequest request, UsersDbContext dbContext, HttpContext httpContext) =>
        {
            var handler = new UpdateUserHandler(dbContext);
            var result = await handler.Handle(new UpdateUserRequest(request.FirstName, request.LastName, request.Email), httpContext.RequestAborted);
            return result.Success ? Results.NoContent() : Results.NotFound();
        });
    }
}