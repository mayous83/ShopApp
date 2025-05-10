using ShopApp.Orders.Service.Database.Migrations;
using ShopApp.Shared.Library.Database;

namespace ShopApp.Orders.Service.Database;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid AddressId { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = [];
}