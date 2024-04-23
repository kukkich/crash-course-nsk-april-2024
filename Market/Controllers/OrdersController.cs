using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Route("orders")]
public class OrdersController : ControllerBase
{
    public OrdersController()
    {
        
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDto product)
    {
        throw new NotImplementedException();
    }
    
    [HttpPost("{orderId:guid}/status")]
    public async Task<IActionResult> SetStatus([FromBody] Guid orderId, [FromBody] OrderStatus newStatus)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{sellerId:guid}")]
    public async Task<IActionResult> GetOrders(Guid sellerId, [FromQuery] OrderSearchType searchOptions)
    {
        throw new NotImplementedException();
    }
}