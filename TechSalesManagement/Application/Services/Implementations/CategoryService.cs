using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Common;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Implementations;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IAuditLogRepository auditLogRepository,
        IUnitOfWork unitOfWork)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Category> CreateCategoryAsync(string name, Guid staffId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("Category name is required.");
        }

        // BR165: Name uniqueness
        var existing = await _categoryRepository.GetByNameAsync(name);
        if (existing != null)
        {
            throw new BadRequestException(MessageConstants.MSG73);
        }

        try
        {
            await _unitOfWork.BeginAsync();

            var category = new Category(name);
            await _categoryRepository.AddAsync(category);

            var auditLog = new AuditLog(staffId, "CREATE_CATEGORY", "Categories", category.id.ToString())
            {
                newValues = System.Text.Json.JsonSerializer.Serialize(new { name = category.name })
            };
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
            
            return category;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteCategoryAsync(Guid id, Guid replacementCategoryId, Guid staffId)
    {
        if (id == replacementCategoryId)
        {
            throw new BadRequestException("Replacement category cannot be the same as the one being deleted.");
        }

        var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
        if (categoryToDelete == null)
        {
            throw new NotFoundException("Category to delete not found.");
        }

        var replacementExists = await _categoryRepository.ExistsAsync(replacementCategoryId);
        if (!replacementExists)
        {
            throw new BadRequestException(MessageConstants.MSG75);
        }

        try
        {
            await _unitOfWork.BeginAsync();

            // UC-41: Migrate products before deletion
            await _productRepository.MigrateProductsAsync(id, replacementCategoryId);

            // Delete the old category
            await _categoryRepository.DeleteAsync(id);

            var auditLog = new AuditLog(staffId, "DELETE_CATEGORY", "Categories", id.ToString())
            {
                oldValues = System.Text.Json.JsonSerializer.Serialize(new { name = categoryToDelete.name }),
                newValues = System.Text.Json.JsonSerializer.Serialize(new { migratedTo = replacementCategoryId }),
                affectedColumns = "ALL"
            };
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        return await _categoryRepository.GetAllAsync();
    }
}
