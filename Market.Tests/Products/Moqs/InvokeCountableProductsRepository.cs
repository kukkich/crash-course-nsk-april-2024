using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO.Products;
using Market.Misc;
using Market.Models.Products;

namespace Market.Tests.Products.Moqs;

public class InvocationsCountableProductsRepository : IProductsRepository
{
    public int SearchProductsInvocationCount { get; private set; }

    public Task<Result<IReadOnlyCollection<Product>, Error>> GetProductsAsync(string? name = null, Guid? sellerId = null, ProductCategory? category = null, int skip = 0,
        int take = 50)
    {
        SearchProductsInvocationCount++;
        Result<IReadOnlyCollection<Product>, Error> result = new List<Product>();
        return Task.FromResult(result);
    }

    public Task<Result<Product, Error>> GetProductAsync(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product, Error>> CreateProductAsync(CreateProductDto productDto, Guid sellerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit, Error>> UpdateProductAsync(Guid productId, Guid sellerId, ProductUpdateInfo updateInfo)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit, Error>> DeleteProductAsync(Guid productId, Guid sellerId)
    {
        throw new NotImplementedException();
    }
}