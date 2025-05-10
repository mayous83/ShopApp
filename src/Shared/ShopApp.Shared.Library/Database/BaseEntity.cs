namespace ShopApp.Shared.Library.Database;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public Guid EntityId { get; set; }
}