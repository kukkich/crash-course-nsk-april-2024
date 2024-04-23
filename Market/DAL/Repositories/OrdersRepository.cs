namespace Market.DAL.Repositories;

internal class OrdersRepository
{
    private readonly RepositoryContext _dbContext;

    public OrdersRepository(RepositoryContext dbContext)
    {
        _dbContext = dbContext;
    }
}