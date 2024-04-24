using Market.Enums;

namespace Market.DTO;

public class CreateProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal PriceInRubles { get; set; }

    public ProductCategory Category { get; set; }
}