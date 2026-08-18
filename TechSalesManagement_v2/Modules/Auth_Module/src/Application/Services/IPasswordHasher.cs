namespace Auth_Module.Application.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
}