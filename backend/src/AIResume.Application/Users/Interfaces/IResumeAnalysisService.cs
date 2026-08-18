using AIResume.Application.Resumes.DTOs;

namespace AIResume.Application.Resumes.Interfaces;

public interface IResumeAnalysisService
{
    Task<ResumeAnalysisResponse> AnalyzeAsync(string resumeText);
}