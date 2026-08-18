using AIResume.Application.Users.Repositories;
using AIResume.Domain.Entities;
using AIResume.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIResume.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.Users
            .AnyAsync(user => user.Email == email);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users
            .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }
}