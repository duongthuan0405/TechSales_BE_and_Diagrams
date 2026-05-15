using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Common;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Domain.Enums;

namespace TechSalesManagement.Application.Services.Implementations;

public class AdminUserService : IAdminUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IAuditLogRepository _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AdminUserService(
        IUserRepository userRepository,
        IAuditLogRepository auditLogRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<(List<User> items, int totalCount)> GetCustomersAsync(int pageNumber, int pageSize)
    {
        return await _userRepository.GetPagedUsersByRoleAsync("Customer", pageNumber, pageSize);
    }

    public async Task LockCustomerAsync(Guid userId, DateTimeOffset? until, Guid staffId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found.");

        try
        {
            await _unitOfWork.BeginAsync();

            user.LockAccount(until);
            await _userRepository.UpdateStatusAsync(userId, UserStatus.BLOCKED, until);

            var auditLog = new AuditLog(staffId, "LOCK_USER", "Users", $"UserId: {userId}, Until: {until}");
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
            
            // Part of Observer: Logic to invalidate sessions could be added here
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task UnlockCustomerAsync(Guid userId, Guid staffId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new NotFoundException("User not found.");

        try
        {
            await _unitOfWork.BeginAsync();

            user.UnlockAccount();
            await _userRepository.UpdateStatusAsync(userId, UserStatus.ACTIVE, null);

            var auditLog = new AuditLog(staffId, "UNLOCK_USER", "Users", userId.ToString());
            await _auditLogRepository.AddAsync(auditLog);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
