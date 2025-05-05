using Microsoft.EntityFrameworkCore;
using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.GetUserById;

public class GetUserByIdHandler
{
    private readonly UsersDbContext _dbContext;
    
    public GetUserByIdHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> Handle(GetUserByIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.EntityId == request.Id, cancellationToken);
        return user;
    }
}