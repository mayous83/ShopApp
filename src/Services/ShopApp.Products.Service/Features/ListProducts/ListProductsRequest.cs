using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Products.Service.Features.ListProducts;

public sealed record ListProductsRequest(string? Search, int Page = 1, int PageSize = 10) : BaseListRequest(Search, Page, PageSize);