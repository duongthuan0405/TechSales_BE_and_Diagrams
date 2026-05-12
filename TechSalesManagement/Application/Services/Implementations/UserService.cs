using System;
using System.Threading.Tasks;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public UserService(IUserRepository userRepository, IUserProfileRepository userProfileRepository)
    {
        _userRepository = userRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null)
        {
            // Nạp động Profile rời rạc phục vụ cho tổng hợp thông tin cá nhân
            user.profile = await _userProfileRepository.GetByUserIdAsync(userId);
        }
        return user;
    }
}
