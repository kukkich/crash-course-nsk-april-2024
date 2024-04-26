using Market.DTO.Products;
using Market.Models.Products;
using Market.Models;

namespace Market.Modules.Carts.DTO;

public class ProductItemsDto
{
    public Guid Id { get; set; }
    public int Count { get; set; }

    public ProductDto Product { get; set; } = null!;
}