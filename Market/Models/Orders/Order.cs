namespace Market.Models.Orders;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    public List<OrderedProductItem> Items { get; set; }
}