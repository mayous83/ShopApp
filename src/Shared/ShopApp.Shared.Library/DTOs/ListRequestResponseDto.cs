using ShopApp.Shared.Library.Database;

namespace ShopApp.Shared.Library.DTOs;

public record ListRequestResponseDto<TEntity>(
    int TotalCount,
    int Page,
    int PageSize,
    IEnumerable<TEntity> Items
) where TEntity : BaseEntity;