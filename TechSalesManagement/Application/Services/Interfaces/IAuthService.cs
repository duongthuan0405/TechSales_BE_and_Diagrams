using System;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(User newUser);
    Task<User> LoginAsync(string email, string password);
    Task VerifyEmailAsync(string email, string token);
    Task ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(string email, string token, string newPassword);
    Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);
}
