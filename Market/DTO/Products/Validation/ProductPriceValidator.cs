using FluentValidation;

namespace Market.DTO.Products.Validation;

public class ProductPriceValidator : AbstractValidator<decimal>
{
    public ProductPriceValidator()
    {
        RuleFor(x => x)
            .GreaterThan(0);
    }
}