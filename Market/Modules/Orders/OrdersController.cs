using Market.DTO;
using Market.Misc;
using Market.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Market.Modules.Orders;

[ApiController]
[Route("orders")]
public class OrdersControllers : ControllerBase
{
    private IOrdersRepository OrdersRepository { get; }

    public OrdersControllers(IOrdersRepository ordersRepository)
    {
        OrdersRepository = ordersRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrder)
    {
        //todo создание ordered item для каждого товара и т.д....
        throw new NotImplementedException();
        //var result = await OrdersRepository.CreateOrderAsync(new Order
        //{
        //    Id = createOrder.Id,
        //    CustomerId = createOrder.CustomerId,
        //});

        //return ParserDbResult.DbResultIsSuccessful(result, out var error)
        //    ? Ok()
        //    : error;
    }

    [HttpPost("items/{orderItemId:guid}/set-state")]
    public async Task<IActionResult> SetState([FromRoute] Guid orderItemId, [FromBody] OrderState state)
    {
        var result = await OrdersRepository.ChangeStateForOrder(orderItemId, state);

        return ParserDbResult.DbResultIsSuccessful(result, out var error)
            ? Ok()
            : error;
    }

    [HttpGet("items/{sellerId:guid}")]
    public async Task<IActionResult> GetOrders([FromRoute] Guid sellerId, [FromQuery] bool onlyCreated)
    {
        throw new NotImplementedException();

        var result = await OrdersRepository.GetOrdersForSeller(sellerId, onlyCreated);

        // return result.MatchActionResult(
        // orders => new JsonResult(orders.Select(CreateOrderDto.FromModel)));
    }
}