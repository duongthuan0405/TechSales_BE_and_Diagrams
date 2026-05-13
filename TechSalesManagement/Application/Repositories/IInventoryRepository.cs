using System;
using System.Threading.Tasks;

namespace TechSalesManagement.Application.Repositories;

public interface IInventoryRepository
{
    Task ReserveStockAsync(Guid productId, int quantity);
}
