using Auth_Module.Infrastructure.Services;

namespace Auth_Module.Application.Services;

public interface IOtpService
{
    OtpResult GenerateOtp();
}