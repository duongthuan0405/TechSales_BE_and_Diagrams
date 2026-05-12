using System;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetByIdAsync(Guid userId);
}
