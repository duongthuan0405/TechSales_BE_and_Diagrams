using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_Module.src.Application.Services
{
    public interface IExecuteAtomically
    {
        Task<T> ExecuteAtomically<T>(Func<Task<T>> action);
    }
}