using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Repositories;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
}
