using AIResume.Application.Jobs.DTOs;

namespace AIResume.Application.Jobs.Interfaces;

public interface IJobMatchService
{
    Task<JobMatchResponse> MatchAsync(
        string resumeText,
        string jobDescription);
}