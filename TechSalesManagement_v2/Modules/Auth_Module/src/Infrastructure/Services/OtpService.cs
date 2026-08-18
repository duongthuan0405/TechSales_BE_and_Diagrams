using Auth_Module.Application.Services;

namespace Auth_Module.Infrastructure.Services;

public class OtpResult 
{
    public string Otp {get; set;} = string.Empty;
    public DateTimeOffset ExpiredAt {get; set;}
}

public class OtpService : IOtpService
{
    public OtpResult GenerateOtp()
    {
        throw new NotImplementedException();
    }
}