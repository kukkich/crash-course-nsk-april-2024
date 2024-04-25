using FluentValidation;
using Market.DTO.Products.Validation;

namespace Market.Tests.Products.Validation;

public class ProductDescriptionTests
{
    private IValidator<string?> validator;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        validator = new ProductDescriptionValidator();
    }

    [Test]
    public void NullDescriptionShouldPass()
    {
        string? description = null;

        var validationResult = validator.Validate(description);

        Assert.That(
            validationResult.IsValid,
            Is.True
        );
    }
}