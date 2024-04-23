using Market.DAL;
using Market.DAL.Repositories;
using Market.Misc;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("customers/{customerId:guid}/cart")]
public class CartsControllers : ControllerBase
{
    public CartsControllers()
    {
        CartsRepository = new CartsRepository();
    }

    private CartsRepository CartsRepository { get; }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsAsync([FromRoute] Guid customerId)
    {
        var result = await CartsRepository.GetCartAsync(customerId);

        return result.MatchActionResult(cart => new JsonResult(cart));
    }
    
    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductAsync([FromRoute] Guid customerId, [FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, false);

        return result.MatchActionResult(_ => Ok());
    }
    
    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProductAsync(Guid customerId, [FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, true);

        return result.MatchActionResult(_ => Ok());
    }
    
    [HttpPost("clear")]
    public async Task<IActionResult> ClearAsync(Guid customerId)
    {
        var result = await CartsRepository.ClearAll(customerId);

        return result.MatchActionResult(_ => Ok());
    }
}