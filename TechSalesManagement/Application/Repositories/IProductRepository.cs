using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Application.Enums;

namespace TechSalesManagement.Application.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetProductsAsync(string? keyword, List<Guid>? categoryIds, SortOrder? sortOrder);
    Task<Product?> GetByIdAsync(Guid id);
    Task MigrateProductsAsync(Guid oldCategoryId, Guid newCategoryId);
}
