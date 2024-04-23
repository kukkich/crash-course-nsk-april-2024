using Market.Misc;
using Market.Models;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal sealed class CartsRepository
{
    private readonly RepositoryContext _context;

    public CartsRepository()
    {
        _context = new RepositoryContext();
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
                cart.ProductIds = new List<Guid>(cart.ProductIds);
                cart.ProductIds.Remove(productId);
            }
            else
            {
                cart.ProductIds = new List<Guid>(cart.ProductIds) { productId };
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
            cart.ProductIds = new List<Guid>();
            
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