using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Orders.Service.Features.ListOrders;

public record ListOrdersRequest(string? Search, int Page, int PageSize) : BaseListRequest(Search, Page, PageSize);