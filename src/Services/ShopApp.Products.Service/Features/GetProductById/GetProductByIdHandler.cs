using Microsoft.EntityFrameworkCore;
using ShopApp.Products.Service.Database;
using ShopApp.Shared.Library;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Products.Service.Features.GetProductById;

public class GetProductByIdHandler
{
    private readonly ProductsDbContext _db;

    public GetProductByIdHandler(ProductsDbContext db)
    {
        _db = db;
    }

    public async Task<Result<ProductResponseDto>> Handle(Guid id, CancellationToken ct)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.EntityId == id, ct);
        if (product is null)
        {
            return Result<ProductResponseDto>.FailureResult("Product not found");
        }

        var productDto = new ProductResponseDto(product.EntityId, product.Name, product.Description, product.Price,
            product.StockQuantity, product.ReservedStockQuantity);
        
        return Result<ProductResponseDto>.SuccessResult(productDto);
    }
}