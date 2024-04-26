using FluentValidation;
using FluentValidation.Results;
using Market.Controllers;
using Market.DAL.Repositories;
using Market.DTO.Products.Validation;
using Market.DTO.Products;
using Market.Models;
using NSubstitute;
using Market.Tests.Products.Moqs;

namespace Market.Tests.Products;

public class ControllerTests
{
    private IValidator<CreateProductDto> validator;
    private CreateProductDto validCreateProduct;
    private IValidator<CreateProductDto> moqValidator;
    private SearchProductRequestDto searchRequest;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        validator = new CreateProductValidator();
        searchRequest = new SearchProductRequestDto(null, null, null);
        moqValidator = Substitute.For<IValidator<CreateProductDto>>();
        moqValidator.Validate(null!)
            .ReturnsForAnyArgs(new ValidationResult());
    }

    [SetUp]
    public void SetUp()
    {
        validCreateProduct = Any.ValidCreateProductDto();
        validator = new CreateProductValidator();
    }

    [Test]
    public async Task NSubstituteMoqRepositoryTest()
    {
        ////arrange
        //var productsRepository = Substitute.For<IProductsRepository>();
        
        //productsRepository.GetProductsAsync()
        //    .ReturnsForAnyArgs(new List<Product>());
        //var productsController = new ProductsController(productsRepository, new UpdateProductRequestValidator());

        ////act
        //await productsController.CreateProductAsync(new CreateProductDto());

        ////assert
        //await productsRepository.GetProductsAsync().ReceivedWithAnyArgs(1);
    }

    [Test]
    public async Task MoqRepositoryTest()
    {
        //arrange
        var productsRepository = new InvocationsCountableProductsRepository();
        var validator = new AlwaysPassedValidator();
        
        var productsController = new ProductsController(productsRepository, validator);

        //act
        await productsController.SearchProductsAsync(searchRequest);

        //assert
        Assert.That(
            productsRepository.SearchProductsInvocationCount, 
            Is.EqualTo(1)
        );
    }
}