using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO.Products;
using Market.Misc;
using Market.Models;

namespace Market.Tests.Products.Moqs;

public class InvocationsCountableProductsRepository : IProductsRepository
{
    public int SearchProductsInvocationCount { get; private set; }

    public Task<Result<IReadOnlyCollection<Product>, DbError>> GetProductsAsync(string? name = null, Guid? sellerId = null, ProductCategory? category = null, int skip = 0,
        int take = 50)
    {
        SearchProductsInvocationCount++;
        Result<IReadOnlyCollection<Product>, DbError> result = new List<Product>();
        return Task.FromResult(result);
    }

    public Task<Result<Product, DbError>> GetProductAsync(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Product, DbError>> CreateProductAsync(CreateProductDto productDto, Guid sellerId)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit, DbError>> UpdateProductAsync(Guid productId, Guid sellerId, ProductUpdateInfo updateInfo)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Unit, DbError>> DeleteProductAsync(Guid productId, Guid sellerId)
    {
        throw new NotImplementedException();
    }
}