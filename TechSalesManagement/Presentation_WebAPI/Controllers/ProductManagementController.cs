using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;
using TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;
using TechSalesManagement.Presentation_WebAPI.Extensions;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Common;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/admin/products")]
[Authorize(Roles = "Staff,Business Admin,Technical Admin")]
public class ProductManagementController : ControllerBase
{
    private readonly IProductManagementService _productManagementService;

    public ProductManagementController(IProductManagementService productManagementService)
    {
        _productManagementService = productManagementService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiSuccessResponse<PagedResponseDto<Product>>>> GetAdminProductsAsync([FromQuery] AdminProductSearchRequestDto request)
    {
        var (products, totalCount) = await _productManagementService.GetAdminProductsAsync(
            request.keyword, request.categoryId, request.status, request.pageNumber, request.pageSize);

        var response = new PagedResponseDto<Product>
        {
            items = products,
            totalCount = totalCount,
            pageNumber = request.pageNumber,
            pageSize = request.pageSize
        };

        return Ok(new ApiSuccessResponse<PagedResponseDto<Product>>(response, "Product list retrieved successfully."));
    }

    [HttpPost]
    public async Task<ActionResult<ApiSuccessResponse<Product>>> CreateAsync([FromBody] CreateProductRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        var images = request.images.Select(img => new ProductImage
        {
            imageUrl = img.imageUrl,
            isPrimary = img.isPrimary
        }).ToList();

        var product = await _productManagementService.CreateProductAsync(
            request.name, request.description, request.price, request.brand, request.categoryId, request.initialStock, images, staffId.Value);

        return Ok(new ApiSuccessResponse<Product>(product, MessageConstants.MSG72));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateProductRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        var images = request.images.Select(img => new ProductImage
        {
            imageUrl = img.imageUrl,
            isPrimary = img.isPrimary
        }).ToList();

        await _productManagementService.UpdateProductAsync(
            id, request.name, request.description, request.price, request.brand, request.categoryId, images, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "Product updated successfully."));
    }

    [HttpPatch("{id}/status")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> DiscontinueAsync([FromRoute] Guid id)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _productManagementService.DiscontinueProductAsync(id, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "Product status updated successfully."));
    }

    [HttpPatch("{id}/inventory")]
    public async Task<ActionResult<ApiSuccessResponse<object>>> UpdateStockAsync([FromRoute] Guid id, [FromBody] UpdateInventoryRequestDto request)
    {
        var staffId = User.GetUserId();
        if (staffId == null) return Unauthorized();

        await _productManagementService.UpdateInventoryAsync(id, request.value, request.type, staffId.Value);

        return Ok(new ApiSuccessResponse<object>(null, "Inventory updated successfully."));
    }
}
