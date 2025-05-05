using FluentValidation;

namespace ShopApp.Products.Service.Features.ReserveStockRequest;

public class ReserveStockValidator : AbstractValidator<ReserveStockRequest>
{
    public ReserveStockValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID cannot be empty.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}