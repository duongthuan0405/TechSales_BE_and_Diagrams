using System;
using TechSalesManagement.Domain.Common;

namespace TechSalesManagement.Domain.Entities;

public class Cart : BaseEntity
{
    private Guid _userId;
    private User? _user;
    private List<CartItem> _items = new();

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public User? User
    {
        get => _user;
        set => _user = value;
    }

    public List<CartItem> Items
    {
        get => _items;
        set => _items = value ?? new();
    }

    public Cart(Guid userId) : base()
    {
        UserId = userId;
    }

    public Cart() : base() { }
}
