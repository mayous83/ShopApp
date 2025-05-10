using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Addresses.Service.Features.ListAddresses;

public record ListAddressesRequest(string? Search, int Page, int PageSize) : BaseListRequest(Search, Page, PageSize);