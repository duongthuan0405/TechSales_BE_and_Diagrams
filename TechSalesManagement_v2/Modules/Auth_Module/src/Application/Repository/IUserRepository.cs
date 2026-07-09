using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.src.Domain.Entities;

namespace Auth_Module.src.Application.Repository
{
    public interface IUserRepository
    {
        Task<User?> CheckExistByEmail(string email);
        Task UpdateAsync(User existingUser);
    }
}