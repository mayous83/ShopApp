using ShopApp.Shared.Library.Database;

namespace ShopApp.Users.Service.Database;

public sealed class User : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}