namespace Market.Models.Products;

public class ProductItem
{
    public Guid Id { get; set; }
    public int Count { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public Guid CartId { get; set; }
    public Cart Cart { get; set; } = null!;
}