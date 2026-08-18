namespace Auth_Module.Domain.Entities;

public class RolePermission
{
    private Guid _roleId = Guid.Empty;
    private Guid _permissionRole = Guid.Empty;

    public RolePermission(Guid roleId, Guid permissionRole)
    {
        _roleId = roleId;
        _permissionRole = permissionRole;
    }

    public RolePermission() {}

    public Guid RoleId { get => _roleId; set => _roleId = value; }
    public Guid PermissionRole { get => _permissionRole; set => _permissionRole = value; }
}