using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSalesManagement.Application.Common.Constants;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Presentation_WebAPI.DTOs.Common;
using TechSalesManagement.Presentation_WebAPI.DTOs.RequestDTOs;
using TechSalesManagement.Presentation_WebAPI.Extensions;

namespace TechSalesManagement.Presentation_WebAPI.Controllers;

[ApiController]
[Route("api/user-profile")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;

    public UserProfileController(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequestDto request)
    {
        var userId = User.GetUserId();
        if (userId == null) return Unauthorized();

        await _userProfileService.UpdateProfileAsync(
            userId.Value, 
            request.fullName, 
            request.phone, 
            request.avatarUrl, 
            request.dateOfBirth);

        return Ok(new ApiSuccessResponse<object>(null, MessageConstants.MSG17));
    }
}
