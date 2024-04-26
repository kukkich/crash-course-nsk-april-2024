using Market.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Market.Models;

[PrimaryKey(nameof(CustomerId))]
public class Cart
{
    public Guid CustomerId { get; set; }
    
    public List<ProductItem> Products { get; set; }
}