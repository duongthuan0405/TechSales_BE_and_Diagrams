using Auth_Module.Domain.Enums;
using Auth_Module.Domain.ErrorMessages;

namespace Auth_Module.Domain.Entities;

public class User
{
    private Guid _id = Guid.Empty;
    private DateTimeOffset _createdAt = DateTimeOffset.UtcNow;
    private DateTimeOffset? _updatedAt = null;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private UserStatus _status = UserStatus.PENDING;
    private int _failedLoginAttempts;
    private DateTimeOffset? _lastFailedAt;
    private DateTimeOffset? _lockedUntil;

    public Guid Id 
    { 
        get => _id; 
        set => _id = value; 
    }
    public DateTimeOffset CreatedAt 
    { 
        get => _createdAt; 
        set => _createdAt = value; 
        } 
    public DateTimeOffset? UpdatedAt 
    { 
        get => _updatedAt; 
        set => _updatedAt = value; 
    }

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
                throw new ArgumentException(DomainErrors.User.PasswordInvalid);
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

    public DateTimeOffset? LastFailedAt
    {
        get => _lastFailedAt;
        set => _lastFailedAt = value;
    }

    public DateTimeOffset? LockedUntil
    {
        get => _lockedUntil;
        set => _lockedUntil = value;
    }

    public User(string email, string password)
    {
        this.Email = email;
        this.Password = password;
        this.Status = UserStatus.PENDING;
        this.FailedLoginAttempts = 0;
    }

    public User() { }

    public void LockAccount(DateTimeOffset? until = null)
    {
        this.Status = UserStatus.BLOCKED;
        this.LockedUntil = until;
        this.UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void UnlockAccount()
    {
        this.Status = UserStatus.ACTIVE;
        this.LockedUntil = null;
        this.UpdatedAt = DateTimeOffset.UtcNow;
    }
}
