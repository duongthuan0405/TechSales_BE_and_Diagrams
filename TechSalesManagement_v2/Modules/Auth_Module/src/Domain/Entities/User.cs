using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth_Module.src.Domain.Enums;

namespace Auth_Module.src.Domain.Entities
{
    public class User
{
    public Guid id { get; set; }
    public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? updatedAt { get; set; }
    private string email = string.Empty;
    private string username = string.Empty;
    private string password = string.Empty;
    private UserStatus status = UserStatus.PENDING;
    private int failedLoginAttempts = 0; 
    private DateTimeOffset? lastFailedAt = null;
    private DateTimeOffset? lockedUntil = null;

    public string Email
    {
        get => email;
        set
        {
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                throw new ArgumentException(DomainErrors.User.EmailInvalid);
            _email = value;
        }
    }

    public string password
    {
        get => _password;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.User.PasswordRequired);
            _password = value;
        }
    }

    public UserStatus status
    {
        get => _status;
        set => _status = value;
    }

    public int failedLoginAttempts
    {
        get => _failed_login_attempts;
        set => _failed_login_attempts = value < 0 ? 0 : value;
    }

    public DateTimeOffset? lastFailedAt
    {
        get => _last_failed_at;
        set => _last_failed_at = value;
    }

    public DateTimeOffset? lockedUntil
    {
        get => _locked_until;
        set => _locked_until = value;
    }

    public UserProfile? profile
    {
        get => _profile;
        set => _profile = value;
    }

    public List<Role> roles
    {
        get => _roles;
        set => _roles = value ?? new();
    }

    public List<ShippingAddress> addresses
    {
        get => _addresses;
        set => _addresses = value ?? new();
    }

    public List<Order> orders
    {
        get => _orders;
        set => _orders = value ?? new();
    }

    public User(string email, string password)
    {
        this.email = email;
        this.password = password;
        this.status = UserStatus.PENDING;
        this.failedLoginAttempts = 0;
    }

    public User() { }

    public void LockAccount(DateTimeOffset? until = null)
    {
        this.status = UserStatus.BLOCKED;
        this.lockedUntil = until;
        this.updatedAt = DateTimeOffset.UtcNow;
    }

    public void UnlockAccount()
    {
        this.status = UserStatus.ACTIVE;
        this.lockedUntil = null;
        this.updatedAt = DateTimeOffset.UtcNow;
    }
}
}