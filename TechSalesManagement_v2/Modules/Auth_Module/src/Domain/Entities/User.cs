using Auth_Module.src.Domain.Enums;
using Auth_Module.src.Domain.ErrorMessages;

namespace Auth_Module.src.Domain.Entities
{
    public class User
    {
        public Guid id { get; set; }
        private string email = string.Empty;
        private string username = string.Empty;
        private string password = string.Empty;
        private UserStatus status = UserStatus.PENDING;
        private int failedLoginAttempts = 0; 
        public DateTimeOffset createdAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? updatedAt { get; set; }
        private DateTimeOffset? lastFailedAt = null;
        private DateTimeOffset? lockedUntil = null;
        private DateTimeOffset? deletedAt = null;

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException();
                email = value;
        
            }
        }

        public string Username
        {
            get => username;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException(DomainErrors.User.UsernameInvalid);
                username = value;
        
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 6)
                    throw new ArgumentException(DomainErrors.User.PasswordInvalid);
                password = value;
            }
        }

        public UserStatus Status
        {
            get => status;
            set => status = value;
        }

        public int FailedLoginAttempts
        {
            get => failedLoginAttempts;
            set => failedLoginAttempts = value < 0 ? 0 : value;
        }

        public DateTimeOffset? LastFailedAt
        {
            get => lastFailedAt;
            set => lastFailedAt = value;
        }

        public DateTimeOffset? LockedUntil
        {
            get => lockedUntil;
            set => lockedUntil = value;
        }
        public DateTimeOffset? DeletedAt 
        { 
            get => deletedAt; 
            set => deletedAt = value; 
        }

        public User(string email, string username, string password)
        {
            this.Email = email;
            this.Password = password;
            this.Username = username;
            this.Status = UserStatus.PENDING;
            this.FailedLoginAttempts = 0;
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