using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auth_Module.src.Application.Services
{
    public interface IUnitOfWork
    {
        Task BeginAsync();
        Task FinishAsync();
        Task RollbackAsync();
    }
}