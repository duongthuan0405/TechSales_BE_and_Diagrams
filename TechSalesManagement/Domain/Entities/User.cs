using System;
using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Enums;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class User : BaseEntity
{
    private string _email = string.Empty;
    private string _password = string.Empty;
    private UserStatus _status;
    private int _failedLoginAttempts;
    private DateTime? _lastFailedAt;
    private DateTime? _lockedUntil;

    // Navigation Properties
    private UserProfile? _profile;
    private List<Role> _roles = new();
    private List<ShippingAddress> _addresses = new();
    private List<Order> _orders = new();

    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException(DomainErrors.User.EmailInvalid);
            _email = value;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.User.PasswordRequired);
            _password = value;
        }
    }

    public UserStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public int FailedLoginAttempts
    {
        get => _failedLoginAttempts;
        set => _failedLoginAttempts = value < 0 ? 0 : value;
    }

    public DateTime? LastFailedAt
    {
        get => _lastFailedAt;
        set => _lastFailedAt = value;
    }

    public DateTime? LockedUntil
    {
        get => _lockedUntil;
        set => _lockedUntil = value;
    }

    public UserProfile? Profile
    {
        get => _profile;
        set => _profile = value;
    }

    public List<Role> Roles
    {
        get => _roles;
        set => _roles = value ?? new();
    }

    public List<ShippingAddress> Addresses
    {
        get => _addresses;
        set => _addresses = value ?? new();
    }

    public List<Order> Orders
    {
        get => _orders;
        set => _orders = value ?? new();
    }

    public User(string email, string password) : base()
    {
        Email = email;
        Password = password;
        Status = UserStatus.PENDING;
        FailedLoginAttempts = 0;
    }

    public User() : base() { }
}
