using FluentValidation;
using Market.DTO.Products.Validation;

namespace Market.Tests.Products.Validation;

public class NameValidatorTests
{
    private IValidator<string> validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        validator = new ProductNameValidator();
    }

    [Test]
    public void EmptyNameShouldFail()
    {
        var name = string.Empty;

        var validationResult = validator.Validate(name);

        Assert.That(
            validationResult.Errors[0].ErrorMessage, 
            Is.EqualTo("Name should not be empty")
        );
    }

    [Test]
    public void TooLongNameShouldFail()
    {
        var name = Any.String(51);

        var validationResult = validator.Validate(name);

        Assert.That(
            validationResult.Errors[0].ErrorMessage,
            Is.EqualTo("Name length must be from 1 to 50")
        );
    }

    [Test]
    public void ShortNameShouldFail()
    {
        var name = Any.String(49);

        var validationResult = validator.Validate(name);

        Assert.That(
            validationResult.IsValid,
            Is.True
        );
    }
}