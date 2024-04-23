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

        return ParserDbResult.DbResultIsSuccessful(result, out var error) 
            ? new JsonResult(result.Result) 
            : error;
    }
    
    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductAsync([FromRoute] Guid customerId, [FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, false);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error) 
            ? Ok() 
            : error;
    }
    
    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProductAsync(Guid customerId, [FromBody] Guid productId)
    {
        var result = await CartsRepository.AddOrRemoveProductToCartAsync(customerId, productId, true);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error) 
            ? Ok() 
            : error;
    }
    
    [HttpPost("clear")]
    public async Task<IActionResult> ClearAsync(Guid customerId)
    {
        var result = await CartsRepository.ClearAll(customerId);
        
        return ParserDbResult.DbResultIsSuccessful(result, out var error) 
            ? Ok() 
            : error;
    }
}