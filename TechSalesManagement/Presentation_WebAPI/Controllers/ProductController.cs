using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Common;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Application.Services.Params;
using TechSalesManagement.Application.Enums;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> SearchProductsAsync(
        [FromQuery] string? keyword, 
        [FromQuery] List<System.Guid>? categoryIds,
        [FromQuery] SortOrder? sortOrder)
    {
        var parameters = new SearchProductParams
        {
            Keyword = keyword,
            CategoryIds = categoryIds,
            SortOrder = sortOrder
        };

        var products = await _productService.SearchProductsAsync(parameters);

        var responseDtos = products.Select(p => new ProductResponseDto
        {
            id = p.id,
            name = p.name,
            description = p.description,
            price = p.price,
            brand = p.brand,
            categoryId = p.categoryId,
            images = p.images.Select(img => new ProductImageResponseDto
            {
                id = img.id,
                imageUrl = img.imageUrl,
                isPrimary = img.isPrimary
            }).ToList()
        }).ToList();

        if (!responseDtos.Any())
        {
            // BR41: Return 200-OK with empty array and MSG24
            return Ok(new ApiSuccessResponse<List<ProductResponseDto>>(
                responseDtos, 
                MessageConstants.MSG24
            ));
        }

        // BR42: Return 200-OK with results
        return Ok(new ApiSuccessResponse<List<ProductResponseDto>>(responseDtos));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductDetailsAsync([FromRoute] System.Guid id)
    {
        var parameters = new GetProductDetailsParams
        {
            ProductId = id
        };

        var product = await _productService.GetProductDetailsAsync(parameters);

        var availableQty = product.inventory?.availableQuantity ?? 0;
        
        var response = new ProductDetailResponseDto
        {
            id = product.id,
            name = product.name,
            description = product.description,
            price = product.price,
            brand = product.brand,
            categoryId = product.categoryId,
            images = product.images.Select(img => new ProductImageResponseDto
            {
                id = img.id,
                imageUrl = img.imageUrl,
                isPrimary = img.isPrimary
            }).ToList(),
            stockStatus = availableQty > 0 ? StockStatus.IN_STOCK : StockStatus.OUT_OF_STOCK,
            availableQuantity = availableQty
        };

        return Ok(new ApiSuccessResponse<ProductDetailResponseDto>(response));
    }
}
