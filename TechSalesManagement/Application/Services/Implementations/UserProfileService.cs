using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechSalesManagement.Application.Common.Constants;
using TechSalesManagement.Application.Exceptions;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Application.Services.Interfaces;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Services.Implementations;

public class UserProfileService : IUserProfileService
{
    private readonly IUserProfileRepository _userProfileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserProfileService(
        IUserProfileRepository userProfileRepository, 
        IUnitOfWork unitOfWork)
    {
        _userProfileRepository = userProfileRepository;
        _unitOfWork = unitOfWork;
    }



    public async Task UpdateProfileAsync(Guid userId, string? fullName, string? phone, string? avatarUrl, DateTime? dateOfBirth)
    {
        try
        {
            await _unitOfWork.BeginAsync();

            var profile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (profile == null)
            {
                // Vì khi Đăng ký đã tự động khởi tạo Profile rỗng, 
                // trường hợp không tìm thấy Profile là dữ liệu không đồng nhất.
                throw new NotFoundException(MessageConstants.MSG117); 
            }

            // Xử lý FullName (Bắt buộc)
            if (string.IsNullOrWhiteSpace(fullName))
            {
                if (string.IsNullOrWhiteSpace(profile.fullName)) 
                    throw new BadRequestException(MessageConstants.MSG1);
                else {
                    fullName = profile.fullName; 
                }
            }
            
            if (string.IsNullOrWhiteSpace(phone))
            {
                if (string.IsNullOrWhiteSpace(profile.phone)) 
                    throw new BadRequestException(MessageConstants.MSG16);
                else {
                    phone = profile.phone; 
                }
                
            }

            if(!Regex.IsMatch(phone, @"^[0-9]{10,11}$")) {
                    throw new BadRequestException(MessageConstants.MSG16);
            }

            profile.fullName = fullName;
            profile.phone = phone;            
            if (avatarUrl != null) profile.avatarUrl = avatarUrl;
            if (dateOfBirth != null) profile.dateOfBirth = dateOfBirth;

            await _userProfileRepository.UpdateAsync(profile);

            await _unitOfWork.FinishAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
