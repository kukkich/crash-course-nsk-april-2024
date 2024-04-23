using Market.Controllers.Extensions;
using Market.DAL.Repositories;
using Market.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[Route("customer/{customerId:guid}/carts")]
public class CartsController : ControllerBase
{
    private readonly CartsRepository _cartsRepository;

    public CartsController(CartsRepository cartsRepository)
    {
        _cartsRepository = cartsRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(Guid customerId)
    {
        var cartResult = await _cartsRepository.GetProductsByCustomer(customerId);

        return cartResult.IsSucceed(out var error)
            ? new JsonResult(cartResult.Result)
            : error;
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProduct(Guid customerId, [FromBody] Guid productId)
    {
        var cartResult = await _cartsRepository.AddProduct(customerId, productId);

        return cartResult.IsSucceed(out var error)
            ? new OkResult()
            : error;
    }

    [HttpPost("remove-product")]
    public async Task<IActionResult> RemoveProduct(Guid customerId, [FromBody] Guid productId)
    {
        var cartResult = await _cartsRepository.RemoveProduct(customerId, productId);

        return cartResult.IsSucceed(out var error)
            ? new OkResult()
            : error;
    }

    [HttpPost("clear")]
    public async Task<IActionResult> Clear(Guid customerId)
    {
        var dbResult = await _cartsRepository.Clear(customerId);
        
        return dbResult.IsSucceed(out var error)
            ? new OkResult()
            : error;
    }
}