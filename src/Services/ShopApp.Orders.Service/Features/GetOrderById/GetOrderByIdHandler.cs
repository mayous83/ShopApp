using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Orders.Service.Features.GetOrderById;

public class GetOrderByIdHandler
{
    private readonly OrdersDbContext _dbContext;
    public GetOrderByIdHandler(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Order>> Handle(GetOrderByIdRequest request, CancellationToken cancellationToken)
    {
        //query orders with order items I havent configured navigation properties
        var order = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.EntityId == request.Id, cancellationToken);

        return order is null ? 
            Result<Order>.FailureResult("Order not found") : 
            Result<Order>.SuccessResult(order);
    }
}