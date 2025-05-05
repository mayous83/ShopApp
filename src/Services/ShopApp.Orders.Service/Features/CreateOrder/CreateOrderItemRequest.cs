namespace ShopApp.Orders.Service.Features.CreateOrder;

public class CreateOrderItemRequest
{
    public Guid ProductId { get; set; }  // External reference (ProductsService)
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}