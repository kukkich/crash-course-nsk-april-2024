using FluentValidation;

namespace Market.DTO.Products.Validation;

public class ProductNameValidator : AbstractValidator<string>
{
    public ProductNameValidator()
    {
        RuleFor(x => x)
            .NotEmpty()
            .WithMessage("Name should not be empty");

        RuleFor(x => x)
            .Length(1, 50)
            .WithMessage("Name length must be from 1 to 50");
    }
}