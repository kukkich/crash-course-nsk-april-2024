using Market.Models;

namespace Market.DAL;

public record ProductUpdateInfo
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? PriceInRubles { get; set; }

    public ProductCategory? Category { get; set; }
}