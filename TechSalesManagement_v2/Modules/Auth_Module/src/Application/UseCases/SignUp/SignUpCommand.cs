using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Auth_Module.src.Application.UseCases.SignUp
{
    public class SignUpCommand : IRequest<SignUpCommandResponse>
    {
        public string Email = string.Empty;
        public string Username = string.Empty;
        public string Password = string.Empty;
    }

    public class SignUpCommandResponse
    {
        public bool IsSuccess = false;
        public DateTimeOffset VerifyEmailOTPExpiredAt = DateTimeOffset.Now;
    }
}