using FluentValidation;

namespace ShopApp.Addresses.Service.Features.UpdateAddress;

public class UpdateAddressValidator : AbstractValidator<UpdateAddressRequest>
{
    public UpdateAddressValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage("Street is required.");

        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage("City is required.");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required.");

        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage("PostalCode is required.");

        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage("Country is required.");
    }
}