namespace ShopApp.Addresses.Service.Features.UpdateAddress;

public class UpdateAddressRequest
{
    public Guid Id { get; set; }
    public string? Street { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
    public string? Country { get; set; } = string.Empty;
}