using Market.DTO.Products;
using Market.Misc;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal sealed class ProductsRepository : IProductsRepository
{
    private readonly RepositoryContext _context;

    public ProductsRepository(RepositoryContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Result<IReadOnlyCollection<Product>, DbError>> GetProductsAsync(
        string? name = null, 
        Guid? sellerId = null, 
        ProductCategory? category = null,
        int skip = 0,
        int take = 50
        )
    {
        IQueryable<Product> query = _context.Products;

        if (name is not null)
            query = query.Where(p => p.Name == name);
        if (sellerId.HasValue)
            query = query.Where(p => p.SellerId == sellerId.Value);
        if (category is not null)
            query = query.Where(p => p.Category == category);

        var products = await query.Skip(skip).Take(take).ToListAsync();

        return products;
    }

    public async Task<Result<Product, DbError>> GetProductAsync(Guid productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        
        if (product is null)
        {
            return DbError.NotFound;
        }

        return product;
    }

    public async Task<Result<Product, DbError>> CreateProductAsync(CreateProductDto productDto, Guid sellerId)
    {
        var product = new Product
        {
            Id = productDto.Id,
            SellerId = sellerId,
            Category = productDto.Category,
            Description = productDto.Description,
            Name = productDto.Name,
            PriceInRubles = productDto.PriceInRubles,
        };

        try
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }

    public async Task<Result<Unit, DbError>> UpdateProductAsync(Guid productId, Guid sellerId, ProductUpdateInfo updateInfo)
    {
        var productToUpdate = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (productToUpdate is null)
        {
            return DbError.NotFound;
        }
        if (updateInfo.Name is not null)
        {
            productToUpdate.Name = updateInfo.Name;
        }
        if (updateInfo.Description is not null)
        {
            productToUpdate.Description = updateInfo.Description;
        }
        if (updateInfo.Category.HasValue)
        {
            productToUpdate.Category = updateInfo.Category.Value;
        }
        if (updateInfo.PriceInRubles.HasValue)
        {
            productToUpdate.PriceInRubles = updateInfo.PriceInRubles.Value;
        }

        try
        {
            await _context.SaveChangesAsync();
            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }

    public async Task<Result<Unit, DbError>> DeleteProductAsync(Guid productId, Guid sellerId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return DbError.NotFound;

        try
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }
}

public interface IProductsRepository
{
    public Task<Result<IReadOnlyCollection<Product>, DbError>> GetProductsAsync(
        string? name = null,
        Guid? sellerId = null,
        ProductCategory? category = null,
        int skip = 0,
        int take = 50
    );

    public Task<Result<Product, DbError>> GetProductAsync(Guid productId);
    public Task<Result<Product, DbError>> CreateProductAsync(CreateProductDto productDto, Guid sellerId);
    public Task<Result<Unit, DbError>> UpdateProductAsync(Guid productId, Guid sellerId, ProductUpdateInfo updateInfo);
    public Task<Result<Unit, DbError>> DeleteProductAsync(Guid productId, Guid sellerId);
}
