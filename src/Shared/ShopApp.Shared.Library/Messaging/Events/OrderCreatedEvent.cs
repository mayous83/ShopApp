namespace ShopApp.Shared.Library.Messaging.Events;

public record OrderCreatedEvent
{
    public Guid OrderId { get; init; }
    public Guid UserId  { get; init; }
    public List<OrderItem> Items { get; init; } = new();

    public record OrderItem(Guid ProductId, int Quantity);
}