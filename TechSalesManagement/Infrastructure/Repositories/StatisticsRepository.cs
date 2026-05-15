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
        // Aggregate revenue from orders that are PAID or DELIVERED
        var data = await _dbContext.Orders
            .Where(o => o.created_at >= startDate && o.created_at <= endDate)
            .Where(o => o.status == OrderStatus.DELIVERED || o.status == OrderStatus.APPROVED || o.status == OrderStatus.SHIPPING) 
            // Note: Strictly PAID orders would be better, but based on current statuses, 
            // APPROVED/SHIPPING/DELIVERED usually imply payment or intent to collect.
            .GroupBy(o => o.created_at.Date)
            .Select(g => new RevenueDataPoint
            {
                Date = g.Key,
                Revenue = g.Sum(o => o.total_amount)
            })
            .OrderBy(d => d.Date)
            .ToListAsync();

        return data;
    }
}
