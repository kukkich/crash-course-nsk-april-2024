using Market.Controllers;
using Market.DAL.Repositories;
using Market.Modules.Carts.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Modules.Carts;

[ApiController]
[Route("customers/{customerId:guid}/cart")]
public class CartsControllers : ControllerBase
{
    public CartsControllers(ICartsRepository cartsRepository)
    {
        CartsRepository = cartsRepository;
    }

    private ICartsRepository CartsRepository { get; }

    [HttpGet]
    public async Task<IActionResult> GetProductItems([FromRoute] Guid customerId)
    {
        var result = await CartsRepository.GetCartAsync(customerId);

        return result.MatchActionResult(cart => new JsonResult(cart));
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddProduct(
        [FromRoute] Guid customerId,
        [FromBody] AddProductDto request
        )
    {
        throw new NotImplementedException();
        // var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, false);
        //
        // return result.MatchActionResult(_ => Ok());
    }

    [HttpPost("items/{productItemId:guid}/set-count")]
    public async Task<IActionResult> SetProductCount(
        Guid customerId,
        Guid productItemId,
        [FromBody] SetProductCountDto request
        )
    {
        throw new NotImplementedException();
    }

    [HttpDelete("items/{productItemId:guid}")]
    public async Task<IActionResult> RemoveProduct(Guid customerId, [FromBody] Guid productItemId)
    {
        throw new NotImplementedException();
        // var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, true);
        //
        // return result.MatchActionResult(_ => Ok());
    }


    [HttpPost("clear")]
    public async Task<IActionResult> Clear(Guid customerId)
    {
        var result = await CartsRepository.ClearAll(customerId);

        return result.MatchActionResult(_ => Ok());
    }
}