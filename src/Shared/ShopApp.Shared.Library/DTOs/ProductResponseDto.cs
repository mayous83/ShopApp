namespace ShopApp.Shared.Library.DTOs;

public record ProductResponseDto(Guid Id, string Name, string Description, decimal Price, int Quantity, int ReservedQuantity);