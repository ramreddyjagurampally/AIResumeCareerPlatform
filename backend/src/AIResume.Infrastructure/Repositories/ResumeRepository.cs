using AIResume.Application.Resumes.Repositories;
using AIResume.Domain.Entities;
using AIResume.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIResume.Infrastructure.Repositories;

public class ResumeRepository : IResumeRepository
{
    private readonly AppDbContext _dbContext;

    public ResumeRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Resume resume)
    {
        await _dbContext.Resumes.AddAsync(resume);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Resume>> GetByUserIdAsync(Guid userId)
    {
        return await _dbContext.Resumes
            .Where(resume => resume.UserId == userId)
            .ToListAsync();
    }

    public async Task<Resume?> GetByIdAsync(Guid resumeId)
    {
        return await _dbContext.Resumes
            .FirstOrDefaultAsync(resume => resume.Id == resumeId);
    }
}