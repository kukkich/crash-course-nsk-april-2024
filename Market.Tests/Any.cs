using Market.DTO.Products;
using Market.Models.Products;

namespace Market.Tests;

public static class Any
{
    public static string String()
    {
        return System.Guid.NewGuid().ToString();
    }

    public static Guid Guid()
    {
        return System.Guid.NewGuid();
    }

    public static string ValidName()
    {
        return "wertyuillm2992133";
    }
    public static string String(int length)
    {
        return new string(Enumerable.Range(0, length)
            .Select(_ => Random.Shared.Next(0, 9))
            .Select(x => (char)x)
            .ToArray());
    }
    
    public static CreateProductDto ValidCreateProductDto()
    {
        return new CreateProductDto
        {
            Category = ProductCategory.Food,
            Description = Guid().ToString(),
            Name = Guid().ToString(),
            Id = Guid(),
            PriceInRubles = 10
        };
    }
}