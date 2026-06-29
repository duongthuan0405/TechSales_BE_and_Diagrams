using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_Module.src.Domain.ErrorMessages
{
    public static partial class DomainErrors
    {
        public static class User
        {
            public const string EmailInvalid = "Email format is invalid.";
            public const string PasswordInvalid = "Password is invalid";
            public const string UsernameInvalid = "Username is invalid";
        }
    }
}