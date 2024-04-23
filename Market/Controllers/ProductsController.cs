using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO;
using Market.Enums;
using Market.Misc;
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
        return ParserDbResult.DbResultIsSuccessful(productResult, out var error)
            ? new JsonResult(productResult.Result)
            : error;
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchProductsAsync([FromBody] SearchProductRequestDto requestInfo)
    {
        var productsResult =
            await ProductsRepository.GetProductsAsync(
                requestInfo.ProductName,
                category: requestInfo.Category,
                skip: requestInfo.Skip,
                take: requestInfo.Take);

        if (!ParserDbResult.DbResultIsSuccessful(productsResult, out var error))
            return error;

        if (!requestInfo.SortType.HasValue)
            return new JsonResult(productsResult.Result.Select(ProductDto.FromModel));

        var productDtos = SortProducts(productsResult.Result, requestInfo.SortType.Value, requestInfo.Ascending)
            .Select(ProductDto.FromModel);
        return new JsonResult(productDtos);
    }

    [HttpGet]
    public async Task<IActionResult> GetSellerProductsAsync(
        [FromQuery] Guid sellerId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50)
    {
        var productsResult = await ProductsRepository.GetProductsAsync(sellerId: sellerId, skip: skip, take: take);
        if (!ParserDbResult.DbResultIsSuccessful(productsResult, out var error))
            return error;

        var productDtos = productsResult.Result.Select(ProductDto.FromModel);
        return new JsonResult(productDtos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProductAsync([FromBody] Product product)
    {
        var createResult = await ProductsRepository.CreateProductAsync(product);

        return ParserDbResult.DbResultIsSuccessful(createResult, out var error)
            ? new StatusCodeResult(StatusCodes.Status201Created)
            : error;
    }

    [HttpPut("{productId:guid}")]
    public async Task<IActionResult> UpdateProductAsync([FromRoute] Guid productId,
        [FromBody] UpdateProductRequestDto requestInfo)
    {
        var updateResult = await ProductsRepository.UpdateProductAsync(productId, new ProductUpdateInfo
        {
            Name = requestInfo.Name,
            Description = requestInfo.Description,
            Category = requestInfo.Category,
            PriceInRubles = requestInfo.PriceInRubles
        });

        return ParserDbResult.DbResultIsSuccessful(updateResult, out var error)
            ? Ok()
            : error;
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
        var deleteResult = await ProductsRepository.DeleteProductAsync(productId);

        return ParserDbResult.DbResultIsSuccessful(deleteResult, out var error)
            ? Ok()
            : error;
    }

    private static IEnumerable<Product> SortProducts(IEnumerable<Product> products, SortType sortType,
        bool ascending)
    {
        switch (sortType)
        {
            case SortType.Name:
                return ascending
                    ? products.OrderBy(p => p.Name)
                    : products.OrderByDescending(p => p.Name);
            case SortType.Price:
            default:
                return ascending
                    ? products.OrderBy(p => p.PriceInRubles)
                    : products.OrderByDescending(p => p.PriceInRubles);
        }
    }
}