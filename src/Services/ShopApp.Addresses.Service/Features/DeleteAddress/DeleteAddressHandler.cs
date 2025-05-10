using Microsoft.EntityFrameworkCore;
using ShopApp.Addresses.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Addresses.Service.Features.DeleteAddress;

public class DeleteAddressHandler
{
    
    private readonly AddressesDbContext _db;
    
    public DeleteAddressHandler(AddressesDbContext db)
    {
        _db = db;
    }
    
    public async Task<Result<bool>> Handle(DeleteAddressRequest request, CancellationToken ct)
    {
        var address = await _db.Addresses.FirstOrDefaultAsync(a => a.EntityId == request.Id, ct);
        if (address is null)
        {
            return Result<bool>.FailureResult("Address not found");
        }

        _db.Addresses.Remove(address);
        await _db.SaveChangesAsync(ct);

        return Result<bool>.SuccessResult(true);
    }
}