using FluentValidation;
using Market.DTO.Products;
using Market.DTO.Products.Validation;

namespace Market.Tests.Products.Validation;

public class CreateProductTests
{
    private IValidator<CreateProductDto> validator;
    private CreateProductDto validCreateProduct;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        validator = new CreateProductValidator();
    }

    [SetUp]
    public void SetUp()
    {
        validCreateProduct = Any.ValidCreateProductDto();
        validator = new CreateProductValidator();
    }

    [Test]
    public void SomeValidProductDtoShouldPass()
    {
        var productDto = validCreateProduct;

        var validationResult = validator.Validate(productDto);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyNameShouldFail()
    {
        var productDto = validCreateProduct;
        productDto.Name = string.Empty;

        var validationResult = validator.Validate(productDto);

        Assert.That(
            validationResult.Errors[0].ErrorMessage, 
            Is.EqualTo("Name should not be empty")
        );
    }

    [Test]
    public void LongNameShouldFail()
    {
        var productDto = validCreateProduct;
        productDto.Name = Any.String(51);
        var validationResult = validator.Validate(productDto);

        Assert.That(
            validationResult.Errors[0].ErrorMessage,
            Is.EqualTo("Name length must be from 1 to 50")
        );
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(50)]
    public void ShortNameShouldPass(int nameLength)
    {
        var productDto = validCreateProduct;
        productDto.Name = Any.String(nameLength);
        var validationResult = validator.Validate(productDto);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public void EmptyDescriptionShouldFail()
    {
        var productDto = validCreateProduct;
        productDto.Description = string.Empty;

        var validationResult = validator.Validate(productDto);

        Assert.That(
            validationResult.Errors[0].ErrorMessage,
            Is.EqualTo("Description length must be from 1 to 50")
        );
    }

    [Test]
    public void NullDescriptionShouldPass()
    {
        var productDto = validCreateProduct;
        productDto.Description = null;

        var validationResult = validator.Validate(productDto);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [Test]
    public void LongDescriptionShouldFail()
    {
        var productDto = validCreateProduct;
        productDto.Description = Any.String(51);

        var validationResult = validator.Validate(productDto);

        Assert.That(
            validationResult.Errors[0].ErrorMessage,
            Is.EqualTo("Description length must be from 1 to 50")
        );
    }

    [TestCase(1)]
    [TestCase(10)]
    [TestCase(50)]
    public void ShortDescriptionShouldPass(int descriptionLength)
    {
        var productDto = validCreateProduct;
        productDto.Description = Any.String(descriptionLength);

        var validationResult = validator.Validate(productDto);

        Assert.That(validationResult.IsValid, Is.True);
    }

    [TestCase(-1)]
    [TestCase(0)]
    public void NonPositivePriceShouldFail(decimal price)
    {
        var productDto = validCreateProduct;
        productDto.PriceInRubles = price;

        var validationResult = validator.Validate(productDto);

        Assert.That(
            validationResult.Errors[0].ErrorMessage,
            Is.EqualTo("Price must be positive")
        );
    }

    [TestCase(213)]
    [TestCase(0.00001)]
    public void PositivePriceShouldPass(decimal price)
    {
        var productDto = validCreateProduct;
        productDto.PriceInRubles = price;

        var validationResult = validator.Validate(productDto);

        Assert.That(validationResult.IsValid, Is.True);
    }
}