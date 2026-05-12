using System.ComponentModel.DataAnnotations;

namespace TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;

public class RegisterRequestDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string password { get; set; } = string.Empty;

    [Required]
    [Compare("password")]
    public string confirmPassword { get; set; } = string.Empty;
}

public class LoginRequestDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required]
    public string password { get; set; } = string.Empty;
}

public class ForgotPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;
}

public class VerifyEmailRequestDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required]
    public string token { get; set; } = string.Empty;
}

public class ResetPasswordRequestDto
{
    [Required]
    [EmailAddress]
    public string email { get; set; } = string.Empty;

    [Required]
    public string token { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string newPassword { get; set; } = string.Empty;

    [Required]
    [Compare("newPassword")]
    public string confirmPassword { get; set; } = string.Empty;
}

public class ChangePasswordRequestDto
{
    [Required]
    public string currentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string newPassword { get; set; } = string.Empty;

    [Required]
    [Compare("newPassword")]
    public string confirmPassword { get; set; } = string.Empty;
}
