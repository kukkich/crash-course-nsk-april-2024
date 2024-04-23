using Market.Enums;
using Market.Misc;
using Market.Models;

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
        return new[] {new Cart
        {
            CustomerId = Guid.Parse("1A57231A-F75B-45F9-817A-46FA5A322C82"),
            Products = new List<Product>()
        }};
    }
}