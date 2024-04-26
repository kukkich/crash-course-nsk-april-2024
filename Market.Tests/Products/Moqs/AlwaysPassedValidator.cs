using FluentValidation;
using FluentValidation.Results;
using Market.DTO.Products;

namespace Market.Tests.Products.Moqs;

public class AlwaysPassedValidator : IValidator<SearchProductRequestDto>
{
    public ValidationResult Validate(IValidationContext context)
    {
        return new ValidationResult();
    }

    public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }

    public IValidatorDescriptor CreateDescriptor()
    {
        throw new NotImplementedException();
    }

    public bool CanValidateInstancesOfType(Type type)
    {
        throw new NotImplementedException();
    }

    public ValidationResult Validate(SearchProductRequestDto instance)
    {
        return new ValidationResult();
    }

    public Task<ValidationResult> ValidateAsync(SearchProductRequestDto instance, CancellationToken cancellation = new CancellationToken())
    {
        throw new NotImplementedException();
    }
}