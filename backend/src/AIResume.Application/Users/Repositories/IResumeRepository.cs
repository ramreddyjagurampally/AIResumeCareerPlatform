using AIResume.Domain.Entities;

namespace AIResume.Application.Resumes.Repositories;

public interface IResumeRepository
{
    Task AddAsync(Resume resume);

    Task<List<Resume>> GetByUserIdAsync(Guid userId);

    Task<Resume?> GetByIdAsync(Guid resumeId);
}