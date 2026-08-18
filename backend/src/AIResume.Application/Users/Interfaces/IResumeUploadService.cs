using AIResume.Domain.Entities;

namespace AIResume.Application.Resumes.Interfaces;

public interface IResumeUploadService
{
    Task<Resume> UploadAsync(
        Guid userId,
        string fileName,
        string filePath);
}