using System;
using System.ComponentModel.DataAnnotations;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;

public class AddToCartRequestDto
{
    [Required]
    public Guid productId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int quantity { get; set; }
}

public class UpdateCartItemRequestDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int quantity { get; set; }
}
