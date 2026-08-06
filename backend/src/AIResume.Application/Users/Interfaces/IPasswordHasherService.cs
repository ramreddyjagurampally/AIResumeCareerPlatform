namespace AIResume.Application.Users.Interfaces;

public interface IPasswordHasherService
{
    string HashPassword(string password);
}