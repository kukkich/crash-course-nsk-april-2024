namespace Market.Models;

public class Cart
{
    public Guid CustomerId { get; set; }

    public List<Guid> ProductIds { get; set; } = new();
}