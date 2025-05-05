namespace ShopApp.Products.Service.Features.ReserveStockRequest;

public record ReserveStockRequest(Guid ProductId, int Quantity);