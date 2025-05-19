using Microsoft.EntityFrameworkCore;
using ShopApp.Products.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Products.Service.Features.DeleteProduct;

public class DeleteProductHandler
{
    
    private readonly ProductsDbContext _dbContext;
    
    public DeleteProductHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.EntityId == id, cancellationToken);
        if (product == null)
            return Result<bool>.FailureResult("Product not found");

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}