using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO.Products;
using Market.Misc;
using Market.Models.Products;
using Market.Modules.Carts.DTO;
namespace Market.Modules.Carts;

public class CartsService : ICartsService
{
    private readonly ICartsRepository _cartsRepository;
    private readonly IProductsRepository _productsRepository;

    public CartsService(ICartsRepository cartsRepository, IProductsRepository productsRepository)
    {
        _cartsRepository = cartsRepository;
        _productsRepository = productsRepository;
    }

    public async Task<Result<List<ProductItemsDto>, Error>> GetProductItems(Guid customerId)
    {
        var result = await _cartsRepository.GetProductItems(customerId);

        return result.Match<Result<List<ProductItemsDto>, Error>>(
            items => items.Select(x => new ProductItemsDto
            {
                Id = x.Id,
                Count = x.Count,
                Product = ProductDto.FromModel(x.Product)
            }).ToList(),
            error => error);
    }

    public async Task<Result<ProductItemsDto, Error>> AddProduct(Guid customerId, AddProductDto product)
    {
        var getProductResult = await _productsRepository.GetProductAsync(product.ProductId);
        if (getProductResult.IsFailure)
        {
            return getProductResult.Error;
        }

        var getCartResult = await _cartsRepository.GetCartByUserId(customerId);
        if (getCartResult.IsFailure)
        {
            return getCartResult.Error;
        }

        var productFromDb = getProductResult.Value!;
        var cart = getCartResult.Value!;

        if (cart.Products.Any(x => x.ProductId == product.ProductId))
        {
            return Error.Conflict;
        }

        var newItem = new ProductItem
        {
            Id = Guid.NewGuid(),
            Count = product.Count,
            Product = productFromDb
        };

        cart.Products.Add(newItem);

        var result = await _cartsRepository.SaveCart(cart);

        return result.Match<Result<ProductItemsDto, Error>>(
            _ => new ProductItemsDto
            {
                Count = newItem.Count,
                Id = newItem.Id,
                Product = ProductDto.FromModel(newItem.Product)
            },
            error => error
        );
    }

    public Task<Result<Unit, Error>> Clear(Guid customerId)
    {
        return _cartsRepository.ClearAll(customerId);
    }
}

public interface ICartsService
{
    Task<Result<List<ProductItemsDto>, Error>> GetProductItems(Guid customerId);
    Task<Result<ProductItemsDto, Error>> AddProduct(Guid customerId, AddProductDto product);
    Task<Result<Unit, Error>> Clear(Guid customerId);
}
