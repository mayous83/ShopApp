using ShopApp.Shared.Library.Database;

namespace ShopApp.Orders.Service.Database.Migrations;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}