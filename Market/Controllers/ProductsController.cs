using Market.Controllers.Extensions;
using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductsController : ControllerBase
{
    public ProductsController()
    {
        ProductsRepository = new ProductsRepository();
    }

    private ProductsRepository ProductsRepository { get; }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
        var productResult = await ProductsRepository.GetProductAsync(productId);
        return productResult.IsSucceed(out var error)
            ? new JsonResult(productResult.Result)
            : error;
    }

    [HttpPatch("search")]
    public async Task<IActionResult> SearchProductsAsync([FromBody] ProductSearchOptions productSearchDto)
    {
        var productResult = await ProductsRepository.SearchProduct(productSearchDto);

        return productResult.IsSucceed(out var error)
            ? new JsonResult(productResult.Result)
            : error;
    }

    [HttpGet]
    public async Task<IActionResult> GetSellerProductsAsync(
        [FromQuery] Guid sellerId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var productsResult = await ProductsRepository.GetProductsAsync(sellerId: sellerId, skip: skip, take: take);
        if (!productsResult.IsSucceed(out var error))
            return error;

        var productDtos = productsResult.Result.Select(ProductDto.FromModel);
        return new JsonResult(productDtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
    {
        var createResult = await ProductsRepository.CreateProductAsync(product);

        return createResult.IsSucceed(out var error)
            ? new StatusCodeResult(StatusCodes.Status205ResetContent)
            : error;
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId, [FromBody] UpdateProductRequestDto requestInfo)
    {
        var updateResult = await ProductsRepository.UpdateProductAsync(productId, new ProductUpdateInfo
        {
            Name = requestInfo.Name,
            Description = requestInfo.Description,
            Category = requestInfo.Category,
            PriceInRubles = requestInfo.PriceInRubles
        });

        return updateResult.IsSucceed(out var error)
            ? new OkResult()
            : error;
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
        var deleteResult = await ProductsRepository.DeleteProductAsync(productId);

        return deleteResult.IsSucceed(out var error)
            ? new OkResult()
            : error;
    }

    // private static bool IsSucceed(DbResult dbResult, out IActionResult error) =>
    //     DbResultStatusIsSuccessful(dbResult.Status, out error);
    //
    // private static bool IsSucceed<T>(DbResult<T> dbResult, out IActionResult error) =>
    //     DbResultStatusIsSuccessful(dbResult.Status, out error);
    //
    // private static bool DbResultStatusIsSuccessful(DbResultStatus status, out IActionResult error)
    // {
    //     error = null!;
    //     switch (status)
    //     {
    //         case DbResultStatus.Ok:
    //             return true;
    //         case DbResultStatus.NotFound:
    //             error = new NotFoundResult();
    //             return false;
    //         default:
    //             error = new StatusCodeResult(StatusCodes.Status500InternalServerError);
    //             return false;
    //     }
    // }
}