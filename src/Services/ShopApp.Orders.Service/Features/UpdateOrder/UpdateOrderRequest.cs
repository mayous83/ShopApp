using ShopApp.Orders.Service.Features.CreateOrder;

namespace ShopApp.Orders.Service.Features.UpdateOrder;

public class UpdateOrderRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public List<CreateOrderItemRequest> Items { get; set; } = [];
}