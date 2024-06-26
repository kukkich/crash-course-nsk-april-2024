﻿using System.Security.Claims;
using Market.Authentication;
using Market.DAL;
using Market.DAL.Repositories;
using Market.DTO.Products;
using Market.DTO.Products.Validation;
using Market.Enums;
using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers;

[ApiController]
[Route("products")]
public sealed class ProductsController : ControllerBase
{
    private IProductsRepository ProductsRepository { get; }

    public ProductsController(IProductsRepository productsRepository)
    {
        ProductsRepository = productsRepository;
    }

    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetProductByIdAsync(Guid productId)
    {
        var productResult = await ProductsRepository.GetProductAsync(productId);
        return productResult.MatchActionResult(Ok);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchProductsAsync([FromBody] SearchProductRequestDto requestInfo)
    {
        var validator = new SearchProductRequestValidator();
        var validationResult = validator.Validate(requestInfo);

        if (!validationResult.IsValid)
        {
            return BadRequest(new {Errors = validationResult});
        }

        var productsResult = await ProductsRepository.GetProductsAsync(
                requestInfo.ProductName,
                category: requestInfo.Category,
                skip: requestInfo.Skip,
                take: requestInfo.Take
           );

        return productsResult.MatchActionResult(products =>
        {
            IEnumerable<ProductDto> productDtos;
            if (requestInfo.SortType.HasValue)
            {
                productDtos = SortProducts(products, requestInfo.SortType.Value, requestInfo.Ascending)
                    .Select(ProductDto.FromModel);
            }
            else
            {
                productDtos = products.Select(ProductDto.FromModel);
            }

            return Ok(productDtos);
        });
    }

    [HttpGet]
    [ServiceFilter(typeof(AuthenticationFilter))]
    // [AuthenticationFilter()]
    public async Task<IActionResult> GetSellerProductsAsync(
        [FromQuery] Guid sellerId,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50
        )
    {
        var validator = new PaginationValidator();
        var validationResult = validator.Validate(new PaginationModel(Take:take, skip));

        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult });
        }

        var productsResult = await ProductsRepository.GetProductsAsync(sellerId: sellerId, skip: skip, take: take);

        return productsResult.MatchActionResult(
            products => Ok(products.Select(ProductDto.FromModel))
        );
    }

    [HttpPost]
    [ServiceFilter(typeof(AuthenticationFilter))]
    // [AuthenticationFilter(acceptOnlySellers:true)]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto product)
    {
        var validator = new CreateProductValidator();
        var validationResult = validator.Validate(product);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult });
        }

        var userId = GetUserId();

        var createResult = await ProductsRepository.CreateProductAsync(product, userId);

        return createResult.MatchActionResult(Ok);
    }

    [HttpPut("{productId:guid}")]
    [ServiceFilter(typeof(AuthenticationFilter))]
    // [AuthenticationFilter(acceptOnlySellers: true)]
    public async Task<IActionResult> UpdateProductAsync(
        [FromRoute] Guid productId,
        [FromBody] UpdateProductRequestDto request
        )
    {
        var validator = new UpdateProductRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(new { Errors = validationResult });
        }

        var userId = GetUserId();

        var updateResult = await ProductsRepository.UpdateProductAsync(
            productId: productId, 
            sellerId: userId, 
            new ProductUpdateInfo 
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                PriceInRubles = request.PriceInRubles
            }
        );

        return updateResult.MatchActionResult(_ => Ok());
    }

    [HttpDelete("{productId:guid}")]
    public async Task<IActionResult> DeleteProductAsync(Guid productId)
    {
        var userId = GetUserId();

        var deleteResult = await ProductsRepository.DeleteProductAsync(
            productId: productId, 
            sellerId: userId
        );

        return deleteResult.MatchActionResult(_ => Ok());
    }

    private static IEnumerable<Product> SortProducts(
        IEnumerable<Product> products, 
        SortType sortType,
        bool ascending
        )
    {
        return sortType switch
        {
            SortType.Name => ascending 
                ? products.OrderBy(p => p.Name) 
                : products.OrderByDescending(p => p.Name),
            SortType.Price => ascending
                ? products.OrderBy(p => p.PriceInRubles)
                : products.OrderByDescending(p => p.PriceInRubles),
            _ => ascending 
                ? products.OrderBy(p => p.PriceInRubles) 
                : products.OrderByDescending(p => p.PriceInRubles)
        };
    }

    private Guid GetUserId()
    {
        var claims = HttpContext.User.Identities.First().Claims.ToList();

        var userIdString = claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
        var userId = Guid.Parse(userIdString);
        return userId;
    }
}