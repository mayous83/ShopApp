using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.GetUserById;

public static class GetUserByIdEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/users/{id:guid}", async (
            Guid id,
            UsersDbContext db
        ) =>
        {
            var handler = new GetUserByIdHandler(db);
            var user = await handler.Handle(new GetUserByIdRequest() {Id = id}, CancellationToken.None);

            return user is not null
                ? Results.Ok(user)
                : Results.NotFound();
        });
    }
}