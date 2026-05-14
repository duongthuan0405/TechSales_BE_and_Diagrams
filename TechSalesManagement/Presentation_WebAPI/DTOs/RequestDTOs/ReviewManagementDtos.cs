using System.ComponentModel.DataAnnotations;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;

public class ReviewReplyRequestDto
{
    [Required]
    public string content { get; set; } = string.Empty;
}

public class ReviewHideRequestDto
{
    [Required]
    public string reason { get; set; } = string.Empty;
}
