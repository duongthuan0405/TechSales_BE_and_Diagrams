namespace Auth_Module.Domain.Entities;

public class UserRole
{
    private Guid _userId = Guid.Empty;
    private Guid _roleId = Guid.Empty;

    public Guid UserId { get => _userId; set => _userId = value; }
    public Guid RoleId { get => _roleId; set => _roleId = value; }

    public UserRole() {}
    
}