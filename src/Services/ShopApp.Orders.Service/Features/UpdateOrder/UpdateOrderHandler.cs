using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Orders.Service.Features.UpdateOrder;

public class UpdateOrderHandler
{
    // private readonly IMapper _mapper;
    private readonly OrdersDbContext _dbContext;
    public UpdateOrderHandler(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.EntityId == request.Id, cancellationToken);
        if (order == null)
        {
            return Result<bool>.FailureResult("Order not found");
        }
        
        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}