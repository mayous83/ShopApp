using Microsoft.EntityFrameworkCore;
using ShopApp.Orders.Service.Database;
using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Orders.Service.Features.ListOrders;

public class ListOrdersHandler {

    private readonly OrdersDbContext _dbContext;
    
    public ListOrdersHandler(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ListRequestResponseDto<Order>> Handle(ListOrdersRequest request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Orders
            .Include(o => o.Items).AsQueryable();
        //ignore search for now query paginated orders
        var totalCount = await query.CountAsync(cancellationToken);
        var orders = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);
        
        return new ListRequestResponseDto<Order>(totalCount, request.Page, request.PageSize, orders);
    }
}