using Microsoft.EntityFrameworkCore;
using ShopApp.Products.Service.Database;
using ShopApp.Shared.Library;

namespace ShopApp.Products.Service.Features.ReserveStockRequest;

public class ReserveStockHandler
{
    private readonly ProductsDbContext _db;
    
    public ReserveStockHandler(ProductsDbContext db)
    {
        _db = db;
    }

    public async Task<Result<bool>> Handle(ReserveStockRequest request, CancellationToken cancellationToken)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.EntityId == request.ProductId, cancellationToken);
        if (product is null)
        {
            return Result<bool>.FailureResult("Product not found");
        }
        
        var availableStock = product.StockQuantity - product.ReservedStockQuantity;

        if (availableStock < request.Quantity)
        {
            return Result<bool>.FailureResult("Not enough stock available");
        }

        product.ReservedStockQuantity += request.Quantity;
        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}