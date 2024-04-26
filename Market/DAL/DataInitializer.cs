using Market.Misc;
using Market.Models;
using Market.Models.Products;

namespace Market.DAL;

internal static class DataInitializer
{
    private static readonly Random Random = Random.Shared;
    private static readonly ProductCategory[] Categories = Enum.GetValues<ProductCategory>();

    public static Product[] InitializeProducts(int count = 10)
    {
        return Enumerable.Range(1, count).Select(number =>
            new Product
            {
                Id = Guid.NewGuid(),
                Name = $"Product-{number}",
                Description = $"Some description for product-{number}",
                PriceInRubles = (decimal)Random.NextDouble(100, 10000),
                Category = Categories[Random.Next(Categories.Length)],
                SellerId = Guid.NewGuid()
            })
            .ToArray();
    }

    public static Cart[] InitializeCarts()
    {
        return new[]
        {
            new Cart
            {
                CustomerId = Guid.Parse("570AA91C-8C51-4C05-803E-E9E2B373A87D")
            }
        };
    }
}