using Microsoft.EntityFrameworkCore;

namespace Market.Models;

[PrimaryKey(nameof(CustomerId))]
public class Cart
{
    public Guid CustomerId { get; set; }

    public List<Product> Products { get; set; } = new();
}