using ShopApp.Shared.Library.Database;

namespace ShopApp.Products.Service.Database;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int ReservedStockQuantity { get; set; }
}