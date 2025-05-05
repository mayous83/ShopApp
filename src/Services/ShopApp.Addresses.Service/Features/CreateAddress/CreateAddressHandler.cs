using ShopApp.Addresses.Service.Database;

namespace ShopApp.Addresses.Service.Features.CreateAddress;

public class CreateAddressHandler
{
    private readonly AddressesDbContext _dbContext;
    
    public CreateAddressHandler(AddressesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Guid> Handle(CreateAddressRequest request, CancellationToken cancellationToken)
    {
        var address = new Address
        {
            EntityId = Guid.NewGuid(),
            Street = request.Street,
            City = request.City,
            State = request.State,
            PostalCode = request.PostalCode,
            Country = request.Country
        };

        _dbContext.Addresses.Add(address);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return address.EntityId;
    }
}