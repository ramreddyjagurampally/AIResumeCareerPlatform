using AIResume.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIResume.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Resume> Resumes => Set<Resume>();
}