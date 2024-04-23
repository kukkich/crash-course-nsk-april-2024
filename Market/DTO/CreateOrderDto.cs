using Market.Enums;
using Market.Models;

namespace Market.DTO;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid SellerId { get; set; }
    public Guid ProductId { get; set; }

    internal static OrderDto FromModel(Order order) =>
        new()
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            SellerId = order.SellerId,
            ProductId = order.ProductId
        };
}