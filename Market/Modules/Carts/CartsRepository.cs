using Market.DAL;
using Market.Misc;
using Market.Models;
using Market.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Market.Modules.Carts;

internal sealed class CartsRepository : ICartsRepository
{
    private readonly RepositoryContext _context;

    public CartsRepository(RepositoryContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Result<List<ProductItem>, Error>> GetProductItems(Guid customerId)
    {
        var cart = await _context.Carts
            .Include(x => x.Products)
            .FirstOrDefaultAsync(p => p.CustomerId == customerId);

        if (cart == null)
        {
            return Error.NotFound;
        }

        return cart.Products;
    }

    public async Task<Result<Unit, Error>> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove)
    {
        var cart = await _context.Carts.FirstOrDefaultAsync(p => p.CustomerId == customerId);
        if (cart == null)
        {
            return Error.NotFound;
        }

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return Error.NotFound;
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
                    .Concat(new[]{new ProductItem
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
            return Error.Unknown;
        }
    }

    public async Task<Result<Unit, Error>> ClearAll(Guid customerId)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(p => p.CustomerId == customerId);

        if (cart == null)
        {
            return Error.NotFound;
        }

        try
        {
            cart.Products = new();

            await _context.SaveChangesAsync();
            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Unknown;
        }
    }

    public async Task<Result<Cart, Error>> GetCartByUserId(Guid customerId)
    {
        var cart = await _context.Carts
            .Include(x => x.Products)
            .FirstOrDefaultAsync(p => p.CustomerId == customerId);

        if (cart == null)
        {
            return Error.NotFound;
        }

        return cart;
    }

    public async Task<Result<Unit, Error>> SaveCart(Cart cart)
    {
        _context.Carts.Update(cart);

        try
        {
            await _context.SaveChangesAsync();
            return Unit.Instance;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Error.Unknown;
        }
    }
}

public interface ICartsRepository
{
    public Task<Result<List<ProductItem>, Error>> GetProductItems(Guid customerId);
    public Task<Result<Unit, Error>> AddOrRemoveProductToCartAsync(Guid customerId, Guid productId, bool isRemove);
    public Task<Result<Unit, Error>> ClearAll(Guid customerId);
    public Task<Result<Cart, Error>> GetCartByUserId(Guid customerId);
    Task<Result<Unit, Error>> SaveCart(Cart cart);
}
