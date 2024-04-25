using Market.Models;

namespace Market.DTO.Products;

public class CreateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal PriceInRubles { get; set; }

    public ProductCategory Category { get; set; }
}