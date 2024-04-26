using Market.Controllers;
using Market.Modules.Carts.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Modules.Carts;

[ApiController]
[Route("customers/{customerId:guid}/cart")]
public class CartsControllers : ControllerBase
{
    private readonly ICartsService _cartsService;
    private readonly ICartsRepository _cartsRepository;

    public CartsControllers(ICartsService cartsService, ICartsRepository cartsRepository)
    {
        _cartsService = cartsService;
        _cartsRepository = cartsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductItems([FromRoute] Guid customerId)
    {
        var result = await _cartsService.GetProductItems(customerId);

        return result.MatchActionResult(cart => new JsonResult(cart));
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddProduct(
        [FromRoute] Guid customerId,
        [FromBody] AddProductDto request
        )
    {
        var result = await _cartsService.AddProduct(customerId, request);
        return result.MatchActionResult(_ => Ok());
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
        // var result = await CartsService.AddOrRemoveProductToCartAsync(customerId, productId, true);
        //
        // return result.MatchActionResult(_ => Ok());
    }


    [HttpPost("clear")]
    public async Task<IActionResult> Clear(Guid customerId)
    {
        var result = await _cartsService.Clear(customerId);

        return result.MatchActionResult(_ => Ok());
    }
}