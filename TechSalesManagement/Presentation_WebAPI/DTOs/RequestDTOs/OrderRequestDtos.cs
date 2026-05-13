using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;

public class PlaceOrderRequestDto
{
    [Required]
    public Dictionary<Guid, int> productsWithQuantity { get; set; } = new();

    [Required]
    public Guid shippingAddressId { get; set; }

    [Required]
    public Guid paymentMethodId { get; set; }

    public string? voucherCode { get; set; }
}
