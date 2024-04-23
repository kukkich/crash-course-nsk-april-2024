namespace Market.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid SellerId { get; set; }
    public Guid CustomerId { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Created,
    Done,
    Canceled
}