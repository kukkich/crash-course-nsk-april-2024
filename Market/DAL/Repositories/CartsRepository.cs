using Market.Misc;
using Market.Models;
using Market.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal sealed class CartsRepository : ICartsRepository
{
    private readonly RepositoryContext _context;

    public CartsRepository(RepositoryContext dbContext)
    {
        _context = dbContext;
    }
    
    public async Task<Result<Cart, DbError>> GetCartAsync(Guid customerId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        if (cart == null)
        {
            return DbError.NotFound;
        }

        return cart;
    }

    public async Task<Result<Unit, DbError>> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
        if (cart == null)
        {
            return DbError.NotFound;
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return DbError.NotFound;
        }
        
        try
        {
            if (isRemove)
            {
                cart.Products = cart.Products
                    .Where(x => x.ProductId == productId)
                    .ToList();
            }
            else
            {
                cart.Products = cart.Products
                    .Concat(new []{new ProductItem
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        Count = 1
                    }})
                    .ToList();
            }
            await _context.SaveChangesAsync();

            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return DbError.Unknown;
        }
    }
    
    public async Task<Result<Unit, DbError>> ClearAll(Guid customerId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        if (cart == null)
        {
            return DbError.NotFound;
        }
        
        try
        {
            cart.Products = new ();
            
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

public interface ICartsRepository
{
    public Task<Result<Cart, DbError>> GetCartAsync(Guid customerId);
    public Task<Result<Unit, DbError>> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove);
    public Task<Result<Unit, DbError>> ClearAll(Guid customerId);
}
