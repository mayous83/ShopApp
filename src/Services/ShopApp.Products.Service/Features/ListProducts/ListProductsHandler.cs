using Microsoft.EntityFrameworkCore;
using ShopApp.Products.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Products.Service.Features.ListProducts;

public class ListProductsHandler
{
    private readonly ProductsDbContext _dbContext;
    
    public ListProductsHandler(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ListRequestResponseDto<Product>> HandleAsync(ListProductsRequest request, CancellationToken cancellationToken) {
        //use IQueryable to filter the products
        var query = _dbContext.Products.AsQueryable();
        
        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(p => p.Name.Contains(request.Search) || p.Description.Contains(request.Search));
        }
        
        // Get the total count of products
        var totalCount = await query.CountAsync(cancellationToken);
        
        // Apply pagination
        var products = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        // Create the response
        var response = new ListRequestResponseDto<Product>(totalCount, request.Page, request.PageSize, products);
        return response;
    }
}