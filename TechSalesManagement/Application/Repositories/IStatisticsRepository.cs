using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechSalesManagement.Application.Repositories;

public class RevenueDataPoint
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
}

public interface IStatisticsRepository
{
    Task<List<RevenueDataPoint>> GetDailyRevenueAsync(DateTimeOffset startDate, DateTimeOffset endDate);
}
