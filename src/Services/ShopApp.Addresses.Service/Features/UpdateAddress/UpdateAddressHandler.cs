using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Addresses.Service.Features.UpdateAddress;

public class UpdateAddressHandler
{
    
    private readonly AddressesDbContext _dbContext;
    public UpdateAddressHandler(AddressesDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(UpdateAddressRequest request, CancellationToken cancellationToken)
    {
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.EntityId == request.Id, cancellationToken);
        if (address is null)
        {
            return Result<bool>.FailureResult("Address not found");
        }

        var updated = false;
        if(request.Street is not null)
        {
            address.Street = request.Street;
            updated = true;
        }
        
        if(request.City is not null)
        {
            address.City = request.City;
            updated = true;
        }
        
        if(request.State is not null)
        {
            address.State = request.State;
            updated = true;
        }
        
        if(request.PostalCode is not null)
        {
            address.PostalCode = request.PostalCode;
            updated = true;
        }
        
        if(request.Country is not null)
        {
            address.Country = request.Country;
            updated = true;
        }
        
        if (!updated)
        {
            return Result<bool>.FailureResult("No fields to update");
        }

        var rowUpdated = await _dbContext.SaveChangesAsync(cancellationToken);
        return rowUpdated == 0 ? 
            Result<bool>.FailureResult("Failed to update address") : 
            Result<bool>.SuccessResult(true);
    }
}