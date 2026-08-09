using Microsoft.AspNetCore.Authorization;

namespace Common_Module.src.CustomAttributes;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
    {
        Policy = $"Permission:{permission}";
    }
}
