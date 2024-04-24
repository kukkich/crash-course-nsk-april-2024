using Market.Models;

namespace Market.DTO.Products;

public class UpdateProductRequestDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? PriceInRubles { get; set; }

    public ProductCategory? Category { get; set; }
}