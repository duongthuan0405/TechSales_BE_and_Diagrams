using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;

namespace TechSalesManagement.Application.Services.Implementations;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepository _statisticsRepository;

    public StatisticsService(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public async Task<List<RevenueChartDto>> GetDailyRevenueChartAsync(DateTimeOffset startDate, DateTimeOffset endDate)
    {
        // 1. Fetch raw data
        var rawData = await _statisticsRepository.GetDailyRevenueAsync(startDate, endDate);

        // 2. Fill gaps (ensure every day has a point even if 0 revenue)
        var result = new List<RevenueChartDto>();
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
        {
            var point = rawData.FirstOrDefault(d => d.Date == date);
            result.Add(new RevenueChartDto
            {
                Label = date.ToString("yyyy-MM-dd"),
                Value = point?.Revenue ?? 0
            });
        }

        return result;
    }
}
