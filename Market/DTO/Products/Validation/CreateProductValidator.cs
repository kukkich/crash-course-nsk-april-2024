using FluentValidation;

namespace Market.DTO.Products.Validation;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        RuleFor(x => x.Name)
            .SetValidator(new ProductNameValidator());

        RuleFor(x => x.Description)
            .SetValidator(new ProductDescriptionValidator());

        RuleFor(x => x.PriceInRubles)
            .SetValidator(new ProductPriceValidator());
    }
}