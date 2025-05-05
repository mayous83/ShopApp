namespace ShopApp.Orders.Service.Features.CreateOrder;

public class CreateOrderRequest
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; } = [];
}