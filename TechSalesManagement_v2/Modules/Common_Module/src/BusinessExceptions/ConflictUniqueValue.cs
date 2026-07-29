using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common_Module.src.BusinessExceptions
{
    public class ConflictUniqueValueException : BusinessException
    {
        public ConflictUniqueValueException(string message = "", Dictionary<string, List<string>>? errors = null) : base(message, errors)
        {
        }

        public override string Code => "CONFLICT";

        

    }
}