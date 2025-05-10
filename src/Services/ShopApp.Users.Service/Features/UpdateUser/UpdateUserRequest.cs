namespace ShopApp.Users.Service.Features.UpdateUser;

public record UpdateUserRequest(string FirstName, string LastName, string Email)
{
    public Guid Id { get; set; }
}