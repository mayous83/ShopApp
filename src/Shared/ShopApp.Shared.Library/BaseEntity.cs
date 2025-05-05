namespace ShopApp.Shared.Library;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public Guid EntityId { get; set; }
}