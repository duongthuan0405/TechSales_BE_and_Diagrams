namespace Auth_Module.Domain.Entities;

public class Permission
{
    private Guid _id = Guid.Empty;
    private string _code = string.Empty;
    private string? description = null;

    public Permission(Guid id, string code, string? description)
    {
        _id = id;
        _code = code;
        this.description = description;
    }

    public Permission() {}

    public Guid Id 
    { 
        get => _id; 
        set => _id = value; 
    }
    public string Code 
    { 
        get => _code; 
        set => _code = value; 
    }
    public string? Description 
    { 
        get => description; 
        set => description = value; 
    }
}