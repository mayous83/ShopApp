using ShopApp.Shared.Library.DTOs;

namespace ShopApp.Users.Service.Features.ListUsers;

public record ListUsersRequest(string? Search, int Page = 1, int PageSize = 10)
    : BaseListRequest(Search, Page, PageSize);
