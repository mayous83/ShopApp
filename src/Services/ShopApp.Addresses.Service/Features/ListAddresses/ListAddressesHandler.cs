using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Addresses.Service.Features.ListAddresses;

public class ListAddressesHandler
{
    private readonly AddressesDbContext _dbContext;
    
    public ListAddressesHandler(AddressesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ListRequestResponseDto<Address>> Handle(ListAddressesRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Addresses.AsQueryable();
        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(a => a.Street.Contains(request.Search) || a.City.Contains(request.Search));
        }
        var totalCount = await query.CountAsync(cancellationToken);
        var addresses = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new ListRequestResponseDto<Address>(totalCount, request.Page, request.PageSize, addresses);
    }
}