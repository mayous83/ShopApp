using Microsoft.EntityFrameworkCore;
using ShopApp.Shared.Library.DTOs;
using ShopApp.Users.Service.Database;

namespace ShopApp.Users.Service.Features.ListUsers;

public class ListUsersHandler
{
    private readonly UsersDbContext _dbContext;
    
    public ListUsersHandler(UsersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<ListRequestResponseDto<User>>> Handle(ListUsersRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
        {
            query = query.Where(u => u.FirstName.Contains(request.Search) || u.LastName.Contains(request.Search) || u.Email.Contains(request.Search));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var users = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        var response = new ListRequestResponseDto<User>(totalCount, request.Page, request.PageSize, users);
        return Result<ListRequestResponseDto<User>>.SuccessResult(response);
    }
}