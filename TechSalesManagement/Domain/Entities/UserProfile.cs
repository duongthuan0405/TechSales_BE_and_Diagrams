using TechSalesManagement.Domain.Common;
using TechSalesManagement.Domain.Constants;

namespace TechSalesManagement.Domain.Entities;

public class UserProfile : BaseEntity
{
    private Guid _userId;
    private string _fullName = string.Empty;
    private string _phone = string.Empty;
    private string? _avatarUrl;
    private DateTime? _dateOfBirth;

    public Guid UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string FullName
    {
        get => _fullName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.UserProfile.FullNameRequired);
            _fullName = value;
        }
    }

    public string Phone
    {
        get => _phone;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(DomainErrors.UserProfile.PhoneRequired);
            _phone = value;
        }
    }

    public string? AvatarUrl
    {
        get => _avatarUrl;
        set => _avatarUrl = value;
    }

    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = value;
    }

    public UserProfile(Guid userId, string fullName, string phone) : base()
    {
        UserId = userId;
        FullName = fullName;
        Phone = phone;
    }

    public UserProfile() : base() { }
}
