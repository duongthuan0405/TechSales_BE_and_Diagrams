using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common_Module.src.BusinessExceptions
{
    public class NotFoundException : BusinessException
    {
        public override string Code => "NOT_FOUND";


        public NotFoundException(string message = "", Dictionary<string, List<string>>? errors = null) : base(message, errors)
        {

        }

    }
}