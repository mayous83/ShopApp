using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.Library.DTOs;
using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.UpdateUser;

public class UpdateUserHandler
{
    private readonly UsersDbContext _dbContext;
    
    public UpdateUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.EntityId == request.Id, cancellationToken);
        if (user == null)
        {
            return Result<bool>.FailureResult("User not found");
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<bool>.SuccessResult(true);
    }
}