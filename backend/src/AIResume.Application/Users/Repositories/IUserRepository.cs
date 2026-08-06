using AIResume.Domain.Entities;

namespace AIResume.Application.Users.Repositories;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email);

    Task AddAsync(User user);
}