using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.CreateUser;

public class CreateUserHandler
{
    private readonly UsersDbContext _dbContext;
    public CreateUserHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            EntityId = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email
        };
        
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return user;
    }
}