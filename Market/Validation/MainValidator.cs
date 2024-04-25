using FluentValidation;
using Market.DTO.Products;

public class MainValidator
{
    private Dictionary<Type, IValidator> _typeToValidator = new();
    public MainValidator()
    {
        BuildValidators();
    }

    public async Task Validate<T>(T value)
    {
        var validator = GetValidator<T>();
        if (validator is null)
            return;

        var validationResult = await validator.ValidateAsync(value);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);
    }

    private IValidator<T>? GetValidator<T>()
    {
        if (!_typeToValidator.TryGetValue(typeof(T), out var validator))
            return null;

        if (!validator.GetType().IsAssignableTo(typeof(IValidator<T>)))
            return null;

        return (IValidator<T>)validator;
    }

    private void BuildValidators()
    {
        _typeToValidator = new Dictionary<Type, IValidator>()
        {
            //todo register validators
            // [typeof(ProductDto)] = new ProductDtoValidator(),
            // [typeof(SearchProductRequestDto)] = new SearchProductRequestDtoValidator()
        };
    }
}