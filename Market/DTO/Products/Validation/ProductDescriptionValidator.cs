using FluentValidation;

namespace Market.DTO.Products.Validation;

public class ProductDescriptionValidator : AbstractValidator<string?>
{
    public ProductDescriptionValidator()
    {
        RuleFor(x => x)
            .Length(1, 50)
            .When(x => x is not null)
            .WithMessage("Description length must be from 1 to 50");
    }
}