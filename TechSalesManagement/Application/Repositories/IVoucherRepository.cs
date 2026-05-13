using System.Threading.Tasks;
using TechSalesManagement.Domain.Entities;

namespace TechSalesManagement.Application.Repositories;

public interface IVoucherRepository
{
    Task<Voucher?> GetByCodeAsync(string code);
    Task UpdateVoucherAsync(Voucher voucher);
}
