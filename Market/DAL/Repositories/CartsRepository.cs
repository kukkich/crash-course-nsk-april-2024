using Market.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Market.DAL.Repositories;

public class CartsRepository
{
    private readonly RepositoryContext _dbContext;

    public CartsRepository()
    {
        _dbContext = new RepositoryContext();
    }

    public async Task<DbResult<Cart>> GetProductsByCustomer(Guid customerId)
    {
        var carts = await _dbContext.Carts
            .FirstOrDefaultAsync(x => x.CustomerId == customerId);

        return carts is not null
            ? new DbResult<Cart>(carts, DbResultStatus.Ok)
            : new DbResult<Cart>(null!, DbResultStatus.NotFound);
    }

    public async Task<DbResult> AddProduct(Guid customerId, Guid productId)
    {
        return await ExecuteProductAction(customerId, productId, ProductAction.Add);
    }

    public async Task<DbResult> RemoveProduct(Guid customerId, Guid productId)
    {
        return await ExecuteProductAction(customerId, productId, ProductAction.Remove);
    }

    public async Task<DbResult> Clear(Guid customerId)
    {
        var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (cart is null)
        {
            return new DbResult(DbResultStatus.NotFound);
        }

        cart.Products.Clear();

        try
        {
            await _dbContext.SaveChangesAsync();
            return new DbResult(DbResultStatus.Ok);
        }
        catch
        {
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    private async Task<DbResult> ExecuteProductAction(Guid customerId, Guid productId, ProductAction action)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
        if (product is null)
        {
            return new DbResult(DbResultStatus.NotFound);
        }

        var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (cart is null)
        {
            return new DbResult(DbResultStatus.NotFound);
        }

        try
        {
            switch (action)
            {
                case ProductAction.Add:
                    cart.Products.Add(product);
                    break;
                case ProductAction.Remove:
                    cart.Products.Remove(product);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }

            await _dbContext.SaveChangesAsync();
            return new DbResult(DbResultStatus.Ok);
        }
        catch
        {
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    private enum ProductAction
    {
        Add, 
        Remove
    }
}