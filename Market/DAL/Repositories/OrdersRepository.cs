using Market.Misc;
using Market.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace Market.DAL.Repositories;

internal class OrdersRepository : IOrdersRepository
{
    private readonly RepositoryContext _context;

    public OrdersRepository(RepositoryContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<DbResult> CreateOrderAsync(Order order)
    {
        try
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    public async Task<DbResult> ChangeStateForOrder(Guid orderId, OrderState newState)
    {
        //todo изменить чтобы менялось именно у конкретной части заказа
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

        if (order is null)
            return new DbResult(DbResultStatus.NotFound);

        // order.State = newState;

        try
        {
            await _context.SaveChangesAsync();
            return new DbResult(DbResultStatus.Ok);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new DbResult(DbResultStatus.UnknownError);
        }
    }

    public async Task<Result<IReadOnlyCollection<OrderedProductItem>, DbError>> GetOrdersForSeller(Guid sellerId, bool onlyCreated)
    {
        var query = _context.OrderedProductItems
            .Include(item => item.Product)
            .Where(item => item.Product.SellerId == sellerId);

        if (onlyCreated)
        {
            query = query
                .Where(item => item.State == OrderState.Created);
        }

        var orders = await query.ToListAsync();
        return orders;
    }
}

public interface IOrdersRepository
{
    public Task<DbResult> CreateOrderAsync(Order order);
    public Task<DbResult> ChangeStateForOrder(Guid orderId, OrderState newState);
    public Task<Result<IReadOnlyCollection<OrderedProductItem>, DbError>> GetOrdersForSeller(Guid sellerId, bool onlyCreated);
}

