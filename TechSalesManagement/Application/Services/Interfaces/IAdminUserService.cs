using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Interfaces;

public interface IAdminUserService
{
    Task<(List<User> items, int totalCount)> GetCustomersAsync(int pageNumber, int pageSize);
    Task LockCustomerAsync(Guid userId, DateTimeOffset? until, Guid staffId);
    Task UnlockCustomerAsync(Guid userId, Guid staffId);
}
