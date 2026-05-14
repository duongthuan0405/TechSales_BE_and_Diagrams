using System;
using System.Collections.Generic;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;

public class OrderResponseDto
{
    public Guid id { get; set; }
    public OrderStatus status { get; set; }
    public decimal totalAmount { get; set; }
    public DateTimeOffset createdAt { get; set; }
}

public class OrderDetailResponseDto
{
    public Guid id { get; set; }
    public OrderStatus status { get; set; }
    public decimal totalProductAmount { get; set; }
    public decimal shippingFee { get; set; }
    public decimal discountAmount { get; set; }
    public decimal totalAmount { get; set; }
    public string shippingAddressSnapshot { get; set; } = string.Empty;
    public DateTimeOffset createdAt { get; set; }
    public DateTimeOffset? approvedAt { get; set; }
    public DateTimeOffset? shippedAt { get; set; }
    public DateTimeOffset? deliveredAt { get; set; }
    public List<OrderItemResponseDto> items { get; set; } = new();
}

public class OrderItemResponseDto
{
    public Guid productId { get; set; }
    public string productName { get; set; } = string.Empty;
    public string? productImageUrl { get; set; }
    public decimal price { get; set; }
    public int quantity { get; set; }
    public decimal subtotal => price * quantity;
}
