using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common_Module.src.BusinessExceptions
{
    public class BadRequestException : BusinessException
    {
        public BadRequestException(string message = "", Dictionary<string, List<string>>? errors = null) : base(message, errors)
        {
        }

        public override string Code => "BAD_REQUEST";
        
    }
}