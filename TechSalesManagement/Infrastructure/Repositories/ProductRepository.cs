using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Enums;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Infrastructure.Persistence;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly TechSalesDbContext _dbContext;

    public ProductRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetProductsAsync(string? keyword, List<Guid>? categoryIds, SortOrder? sortOrder)
    {
        var query = _dbContext.Products
            .Include(p => p.product_images)
            .Where(p => p.status == ProductStatus.ACTIVE);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var lowercaseKeyword = keyword.ToLower();
            query = query.Where(p => p.name.ToLower().Contains(lowercaseKeyword) 
                                  || p.brand.ToLower().Contains(lowercaseKeyword));
        }

        if (categoryIds != null && categoryIds.Any())
        {
            query = query.Where(p => categoryIds.Contains(p.category_id));
        }

        if (sortOrder != null)
        {
            if (sortOrder == SortOrder.ASC)
            {
                query = query.OrderBy(p => p.price);
            }
            else if (sortOrder == SortOrder.DESC)
            {
                query = query.OrderByDescending(p => p.price);
            }
        }

        var dbModels = await query.ToListAsync();

        return dbModels.Select(MapToEntity).Cast<Product>().ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var dbModel = await _dbContext.Products
            .Include(p => p.product_images)
            .Include(p => p.inventory)
            .FirstOrDefaultAsync(p => p.id == id && p.status == ProductStatus.ACTIVE);

        return MapToEntity(dbModel);
    }

    public async Task MigrateProductsAsync(Guid oldCategoryId, Guid newCategoryId)
    {
        await _dbContext.Products
            .Where(p => p.category_id == oldCategoryId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.category_id, newCategoryId));
    }

    private Product? MapToEntity(ProductDbModel? dbModel)
    {
        if (dbModel == null) return null;

        var product = new Product
        {
            id = dbModel.id,
            name = dbModel.name,
            description = dbModel.description,
            price = dbModel.price,
            status = dbModel.status,
            brand = dbModel.brand,
            categoryId = dbModel.category_id,
            createdAt = dbModel.created_at,
            updatedAt = dbModel.updated_at
        };

        if (dbModel.product_images != null && dbModel.product_images.Any())
        {
            product.images = dbModel.product_images.Select(img => new ProductImage
            {
                id = img.id,
                productId = img.product_id,
                imageUrl = img.image_url,
                isPrimary = img.is_primary,
                createdAt = img.created_at
            }).ToList();
        }

        if (dbModel.inventory != null)
        {
            product.inventory = new Inventory
            {
                productId = dbModel.inventory.product_id,
                quantity = dbModel.inventory.quantity,
                reservedQuantity = dbModel.inventory.reserved_quantity
            };
        }

        return product;
    }
}
