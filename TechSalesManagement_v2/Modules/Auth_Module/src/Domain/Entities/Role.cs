namespace Auth_Module.Domain.Entities;
public class Role
{
    private Guid _id = Guid.Empty;
    private string _name = string.Empty;
    private string _description = string.Empty;

    public Role()
    {
        
    }

    public Role(Guid id, string name)
    {
        _id = id;
        _name = name;
    }

    public Guid Id 
    { 
        get => _id; 
        set => _id = value; 
    }
    public string Name 
    { 
        get => _name; 
        set => _name = value; 
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }
}