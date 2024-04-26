namespace Market.Models.Products;

public sealed class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal PriceInRubles { get; set; }

    public ProductCategory Category { get; set; }

    public Guid SellerId { get; set; }
}