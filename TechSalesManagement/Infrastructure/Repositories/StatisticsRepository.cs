using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Infrastructure.Persistence;

namespace TechSalesManagement.Infrastructure.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly TechSalesDbContext _dbContext;

    public StatisticsRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RevenueDataPoint>> GetDailyRevenueAsync(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        // Aggregate revenue from successful payments
        var data = await _dbContext.Payments
            .Where(p => p.status == PaymentStatus.SUCCESS)
            .Where(p => p.created_at >= startDate && p.created_at <= endDate)
            .GroupBy(p => p.created_at.Date)
            .Select(g => new RevenueDataPoint
            {
                Date = g.Key,
                Revenue = g.Sum(p => p.amount)
            })
            .OrderBy(d => d.Date)
            .ToListAsync();

        return data;
    }
}
