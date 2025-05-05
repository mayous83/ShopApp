using ShopApp.Products.Service.Database;

namespace ShopApp.Products.Service.Features.CreateProduct;

public class CreateProductHandler
{
    private readonly ProductsDbContext _dbContext;
    
    public CreateProductHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Guid> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            EntityId = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity
        };

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return product.EntityId;
    }
}