using Market.Models;

namespace Market.DTO;

public class CreateOrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid SellerId { get; set; }

    internal static CreateOrderDto FromModel(Order order) =>
        new()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            SellerId = order.SellerId,
            ProductId = order.ProductId
        };
}