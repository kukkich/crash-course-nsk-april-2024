using FluentValidation;

namespace Market.DTO.Products.Validation;

public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequestDto>
{
    public UpdateProductRequestValidator()
    {
        RuleFor(x => x.Name!)
            .SetValidator(new ProductNameValidator())
            .When(x => x.Name is not null);

        RuleFor(x => x.Description)
            .SetValidator(new ProductDescriptionValidator());

        RuleFor(x => (decimal)x.PriceInRubles!)
            .SetValidator(new ProductPriceValidator())
            .When(x => x.PriceInRubles is not null);
    }
}