using System;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IUserProfileService
{
    Task UpdateProfileAsync(Guid userId, string? fullName, string? phone, string? avatarUrl, DateTime? dateOfBirth);
}
