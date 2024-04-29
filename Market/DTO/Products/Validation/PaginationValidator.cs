using FluentValidation;

namespace Market.DTO.Products.Validation;

public class PaginationValidator : AbstractValidator<PaginationModel>
{
    public PaginationValidator()
    {
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take)
            .GreaterThan(0)
            .LessThanOrEqualTo(200);
    }
}

public readonly record struct PaginationModel(int Take, int Skip);
