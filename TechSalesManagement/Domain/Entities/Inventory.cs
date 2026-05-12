using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class Inventory : BaseEntity
{
    private Guid _productId;
    private int _quantity;
    private int _reservedQuantity;

    public Guid ProductId
    {
        get => _productId;
        set => _productId = value;
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0) _quantity = 0;
            else _quantity = value;
        }
    }

    public int ReservedQuantity
    {
        get => _reservedQuantity;
        set
        {
            if (value < 0) _reservedQuantity = 0;
            else _reservedQuantity = value;
        }
    }

    // Logic nghiệp vụ: Số lượng thực tế có thể bán
    public int AvailableQuantity => _quantity - _reservedQuantity;

    public Inventory(Guid productId, int quantity) : base()
    {
        ProductId = productId;
        Quantity = quantity;
        ReservedQuantity = 0;
    }

    public Inventory() : base() { }

    // Phương thức nghiệp vụ: Giữ hàng khi có order mới
    public void ReserveStock(int amount)
    {
        if (amount > AvailableQuantity)
            throw new InvalidOperationException(DomainErrors.Inventory.InsufficientStock);
        
        _reservedQuantity += amount;
    }
}
