using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Infrastructure.Persistence;

namespace TechSalesManagement.Infrastructure.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly TechSalesDbContext _dbContext;

    public InventoryRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ReserveStockAsync(Guid productId, int quantityToReserve)
    {
        var dbModel = await _dbContext.Inventories
            .FirstOrDefaultAsync(i => i.product_id == productId);

        if (dbModel != null)
        {
            dbModel.reserved_quantity += quantityToReserve;
            _dbContext.Inventories.Update(dbModel);
        }
    }
}
