using AIResume.Application.Resumes.Interfaces;
using AIResume.Application.Resumes.Repositories;
using AIResume.Domain.Entities;

namespace AIResume.Application.Resumes.Services;

public class ResumeUploadService : IResumeUploadService
{
    private readonly IResumeRepository _resumeRepository;

    public ResumeUploadService(
        IResumeRepository resumeRepository)
    {
        _resumeRepository = resumeRepository;
    }

    public async Task<Resume> UploadAsync(
        Guid userId,
        string fileName,
        string filePath)
    {
        var resume = new Resume(
            userId,
            fileName,
            filePath);

        await _resumeRepository.AddAsync(resume);

        return resume;
    }
}