namespace ShopApp.Shared.Library.DTOs;

public record BaseListRequest(string? Search, int Page = 1, int PageSize = 10)
{
    public string? Search { get; set; } = Search;
    public int Page { get; set; } = Page;
    public int PageSize { get; set; } = PageSize;
}