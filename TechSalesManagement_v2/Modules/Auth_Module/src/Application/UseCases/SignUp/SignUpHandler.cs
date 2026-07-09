using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.src.Application.Repository;
using Auth_Module.src.Application.Services;
using Auth_Module.src.Domain.Entities;
using Auth_Module.src.Domain.Enums;
using MediatR;

namespace Auth_Module.src.Application.UseCases.SignUp
{
    public class SignUpHandler : IRequestHandler<SignUpCommand, SignUpCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public SignUpHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
        {
            this._unitOfWork = unitOfWork;
            this._userRepository = userRepository;
        }
        public async Task<SignUpCommandResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginAsync();
                User registerUser = null!;

                var existingUser = await _userRepository.CheckExistByEmail(request.Email);
                
                if (existingUser != null)
                {
                    if (existingUser.Status != UserStatus.PENDING)
                    {
                        throw new ConflictException(MessageConstants.MSG5);
                    }

                    // Cập nhật mật khẩu mới cho user đang chờ xác nhận
                    existingUser.Password = _passwordHasher.HashPassword(request.Password);
                    existingUser.Email = request.Email;
                    await _userRepository.UpdateAsync(existingUser);
                    registerUser = existingUser;
                }
                else
                {
                    // Tạo mới user nếu chưa tồn tại
                    var newUser = new User(request.Email, request.Password);
                    
                    // Gán mặc định vai trò 'Customer' cho người dùng mới
                    var customerRole = await _roleRepository.GetByNameAsync("Customer");
                    if (customerRole != null)
                    {
                        newUser.roles.Add(customerRole);
                    }

                    // Khởi tạo Profile trống
                    var userProfile = new UserProfile
                    {
                        fullName = string.Empty,
                        phone = string.Empty
                    };

                    await _userRepository.AddAsync(newUser);

                    // Lưu trữ Profile thông qua repository riêng biệt
                    userProfile.userId = newUser.id;
                    await _userProfileRepository.AddAsync(userProfile);

                    userToReturn = newUser;
                }

                // Tạo OTP mới
                var otpResult = _otpService.GenerateOtp();

                // Cập nhật bản ghi OTP cũ nếu có, nếu không thì tạo mới
                var existingToken = await _userTokenRepository.GetByUserIdAndTypeAsync(userToReturn.id, TokenType.EMAIL_VERIFICATION);
                if (existingToken != null)
                {
                    existingToken.token = otpResult.otp;
                    existingToken.expiredAt = otpResult.expiredAt;
                    existingToken.usedAt = null;
                    await _userTokenRepository.UpdateAsync(existingToken);
                }
                else
                {
                    var userToken = new UserToken(userToReturn.id, otpResult.otp, TokenType.EMAIL_VERIFICATION, otpResult.expiredAt);
                    await _userTokenRepository.AddAsync(userToken);
                }

                var verificationLink = $"{_frontendCO.url}/verify-email?email={userToReturn.email}&token={otpResult.otp}";
                await _emailService.SendVerificationEmailAsync(userToReturn.email, verificationLink);

                await _unitOfWork.FinishAsync();
                
                return userToReturn;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }
    }
}