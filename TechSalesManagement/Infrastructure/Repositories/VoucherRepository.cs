using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Infrastructure.Persistence;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Repositories;

public class VoucherRepository : IVoucherRepository
{
    private readonly TechSalesDbContext _dbContext;

    public VoucherRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Voucher?> GetByCodeAsync(string code)
    {
        var dbModel = await _dbContext.Vouchers
            .FirstOrDefaultAsync(v => v.code.ToLower() == code.ToLower());

        return MapToEntity(dbModel);
    }

    public async Task UpdateVoucherAsync(Voucher voucher)
    {
        var dbModel = await _dbContext.Vouchers.FindAsync(voucher.id);
        if (dbModel != null)
        {
            dbModel.used_count = voucher.usedCount;
            dbModel.updated_at = DateTimeOffset.UtcNow;
            _dbContext.Vouchers.Update(dbModel);
        }
    }

    private Voucher? MapToEntity(VoucherDbModel? dbModel)
    {
        if (dbModel == null) return null;

        return new Voucher
        {
            id = dbModel.id,
            createdAt = dbModel.created_at,
            updatedAt = dbModel.updated_at,
            code = dbModel.code,
            type = dbModel.type,
            value = dbModel.value,
            maxUsage = dbModel.max_usage,
            usedCount = dbModel.used_count,
            minOrderAmount = dbModel.min_order_amount,
            startDate = dbModel.start_date,
            endDate = dbModel.end_date,
            isActive = dbModel.is_active
        };
    }
}
