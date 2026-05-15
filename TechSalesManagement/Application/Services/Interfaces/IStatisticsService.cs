using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Repositories;

namespace TechSalesManagement.Application.Services.Interfaces;

public class RevenueChartDto
{
    public string Label { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

public interface IStatisticsService
{
    Task<List<RevenueChartDto>> GetDailyRevenueChartAsync(DateTimeOffset startDate, DateTimeOffset endDate);
}
