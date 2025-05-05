using FluentValidation;
using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.CreateUser;

public static class CreateUserEndpoint
{
    public static void Map(WebApplication app)
    {
        app.MapPost("/users", async (
            CreateUserRequest request,
            IValidator<CreateUserRequest> validator,
            UsersDbContext db
        ) =>
        {
            var validation = await validator.ValidateAsync(request);
            if (!validation.IsValid)
                return Results.BadRequest(validation.Errors);

            var handler = new CreateUserHandler(db);
            //cancellation token from request
            var id = await handler.Handle(request, CancellationToken.None);
            return Results.Created($"/users/{id}", new { UserId = id });
        });
    }
}