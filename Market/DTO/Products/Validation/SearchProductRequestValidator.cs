using FluentValidation;

namespace Market.DTO.Products.Validation;

public class SearchProductRequestValidator : AbstractValidator<SearchProductRequestDto>
{
    public SearchProductRequestValidator()
    {
        RuleFor(x => new PaginationModel(x.Take, x.Skip))
            .SetValidator(new PaginationValidator());
    }
}