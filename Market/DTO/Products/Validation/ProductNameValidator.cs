using FluentValidation;

namespace Market.DTO.Products.Validation;

public class ProductNameValidator : AbstractValidator<string>
{
    public ProductNameValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .Length(1, 50);
    }
}