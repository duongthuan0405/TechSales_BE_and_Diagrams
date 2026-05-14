using System.Threading.Tasks;
using TechSalesManagement.Application.Repositories;
using TechSalesManagement.Domain.Entities;
using TechSalesManagement.Infrastructure.Persistence;
using TechSalesManagement.Infrastructure.Persistence.Models;

namespace TechSalesManagement.Infrastructure.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly TechSalesDbContext _dbContext;

    public AuditLogRepository(TechSalesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AuditLog log)
    {
        var dbModel = new AuditLogDbModel
        {
            id = log.id != System.Guid.Empty ? log.id : System.Guid.NewGuid(),
            user_id = log.userId,
            action = log.action,
            table_name = log.tableName,
            primary_key = log.primaryKey,
            old_values = log.oldValues,
            new_values = log.newValues,
            affected_columns = log.affectedColumns,
            created_at = log.createdAt
        };

        await _dbContext.AuditLogs.AddAsync(dbModel);
    }
}
