using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.Library.DTOs;
using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.DeleteUser;

public class DeleteUserHandler
{
    private readonly UsersDbContext _dbContext;
    
    public DeleteUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.EntityId == request.Id, cancellationToken);
        if (user == null)
        {
            return Result<bool>.FailureResult("User not found");
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}