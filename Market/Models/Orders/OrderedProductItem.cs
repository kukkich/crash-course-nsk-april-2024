using Market.Models.Products;

namespace Market.Models.Orders;

public class OrderedProductItem
{
    public Guid Id { get; set; }
    public int Count { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public OrderState State { get; set; } = OrderState.Created;
}