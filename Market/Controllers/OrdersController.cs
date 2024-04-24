using Market.DAL.Repositories;
using Market.DTO;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("orders")]
public class OrdersControllers : ControllerBase
{
    public OrdersControllers()
    {
        OrdersRepository = new OrdersRepository();
    }

    private OrdersRepository OrdersRepository { get; }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderDto order)
    {
        var result = await OrdersRepository.CreateOrderAsync(new Order()
        {
            Id = order.Id,
            State = OrderState.Created,
            CustomerId = order.CustomerId,
            ProductId = order.ProductId,
            SellerId = order.SellerId
        });

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
    }

    [HttpPost("{orderId:guid}/set-state")]
    public async Task<IActionResult> SetState([FromRoute] Guid orderId, [FromBody] OrderState state)
    {
        var result = await OrdersRepository.ChangeStateForOrder(orderId, state);

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
    }

    [HttpGet("{sellerId:guid}")]
    public async Task<IActionResult> GetOrders([FromRoute] Guid sellerId, [FromQuery] bool onlyCreated)
    {
        var result = await OrdersRepository.GetOrdersForSeller(sellerId, onlyCreated);

        var orderDtos = result.Result.Select(OrderDto.FromModel);
        return new JsonResult(orderDtos);
    }
}