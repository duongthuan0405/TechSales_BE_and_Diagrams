using System.ComponentModel.DataAnnotations;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;

public class UpdateProfileRequestDto
{
    public string? fullName { get; set; }

    [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "MSG16")]
    public string? phone { get; set; }

    public string? avatarUrl { get; set; }
    public DateTime? dateOfBirth { get; set; }
}
