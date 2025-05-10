using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Orders.Service.Features.DeleteOrder;

public class DeleteOrderHandler
{
    private readonly OrdersDbContext _dbContext;
    
    public DeleteOrderHandler(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Result<bool>> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.EntityId == request.Id, cancellationToken);
        if (order == null)
        {
            return Result<bool>.FailureResult("Order not found");
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<bool>.SuccessResult(true);
    }
}