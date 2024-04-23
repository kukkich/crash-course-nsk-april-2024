namespace Market.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid ProductId { get; set; }
    public OrderState State { get; set; } = OrderState.Created;
}