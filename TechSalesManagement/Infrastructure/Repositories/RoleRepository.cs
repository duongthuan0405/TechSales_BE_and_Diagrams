using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechSalesManagement.Application.Interfaces;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Infrastructure.Persistence;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly TechSalesDbContext _dbContext;

    public RoleRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        var dbModel = await _dbContext.Roles
            .FirstOrDefaultAsync(r => r.name == name);

        if (dbModel == null) return null;

        return new Role
        {
            id = dbModel.id,
            name = dbModel.name,
            description = dbModel.description,
            createdAt = dbModel.created_at,
            updatedAt = null // Set if field existed in DbModel
        };
    }
}
