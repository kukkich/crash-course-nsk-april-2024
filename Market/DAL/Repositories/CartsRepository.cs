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
    
    public async Task<DbResult<Cart>> GetCartAsync(Guid customerId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        return cart != null
            ? new DbResult<Cart>(cart, DbResultStatus.Ok)
            : new DbResult<Cart>(null!, DbResultStatus.NotFound);
    }

    public async Task<DbResult> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
        if (cart == null)
        {
            return new DbResult(DbResultStatus.NotFound);
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return new DbResult(DbResultStatus.NotFound);
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

            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }
    
    public async Task<DbResult> ClearAll(Guid customerId)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId.Equals(customerId));

        if (cart == null)
        {
            return new DbResult(DbResultStatus.NotFound);
        }
        
        try
        {
            cart.ProductIds = new List<Guid>();
            
            await _context.SaveChangesAsync();
            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }
}