using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/admin/statistics")]
[Authorize(Roles = "Staff,Business Admin,Technical Admin")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("revenue")]
    public async Task<ActionResult<ApiSuccessResponse<List<RevenueChartDto>>>> GetRevenueChartAsync([FromQuery] DateTimeOffset? startDate, [FromQuery] DateTimeOffset? endDate)
    {
        var end = endDate ?? DateTimeOffset.UtcNow;
        var start = startDate ?? end.AddDays(-30);

        var data = await _statisticsService.GetDailyRevenueChartAsync(start, end);

        return Ok(new ApiSuccessResponse<List<RevenueChartDto>>(data, "Revenue statistics retrieved successfully."));
    }
}
