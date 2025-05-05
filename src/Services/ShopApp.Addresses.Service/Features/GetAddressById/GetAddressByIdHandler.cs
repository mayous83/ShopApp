using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Addresses.Service.Features.GetAddressById;

namespace ShopApp.Addresses.Service.Features.GetAddressById;

public class GetAddressByIdHandler
{
    private readonly AddressesDbContext _dbContext;
    public GetAddressByIdHandler(AddressesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Address?> Handle(GetAddressByIdRequest request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.EntityId == request.Id, cancellationToken);
        return address;
    }
}