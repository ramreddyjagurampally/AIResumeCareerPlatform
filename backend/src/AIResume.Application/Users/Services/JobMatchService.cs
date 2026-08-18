using AIResume.Application.Jobs.DTOs;
using AIResume.Application.Jobs.Interfaces;

namespace AIResume.Application.Jobs.Services;

public class JobMatchService : IJobMatchService
{
    public Task<JobMatchResponse> MatchAsync(
        string resumeText,
        string jobDescription)
    {
        var response = new JobMatchResponse();

        var resume = resumeText.ToLowerInvariant();
        var job = jobDescription.ToLowerInvariant();

        var knownSkills = new[]
        {
            "c#",
            ".net",
            "asp.net core",
            "web api",
            "entity framework",
            "sql",
            "sql server",
            "react",
            "angular",
            "javascript",
            "typescript",
            "azure",
            "aws",
            "docker",
            "kubernetes",
            "git",
            "microservices",
            "terraform",
            "kafka",
            "python"
        };

        // Find skills requested by the job description
        var requiredSkills = knownSkills
            .Where(skill => job.Contains(skill))
            .ToList();

        foreach (var skill in requiredSkills)
        {
            if (resume.Contains(skill))
            {
                response.MatchedSkills.Add(skill);
            }
            else
            {
                response.MissingSkills.Add(skill);
            }
        }

        // Calculate match score
        if (requiredSkills.Count > 0)
        {
            response.MatchScore =
                (int)Math.Round(
                    (double)response.MatchedSkills.Count /
                    requiredSkills.Count * 100);
        }
        else
        {
            response.MatchScore = 0;

            response.Recommendations.Add(
                "No recognized technical skills were found in the job description.");
        }

        // Recommendations
        foreach (var skill in response.MissingSkills)
        {
            response.Recommendations.Add(
                $"Consider highlighting {skill} experience if you have it.");
        }

        if (response.MatchScore >= 80)
        {
            response.Recommendations.Add(
                "Your resume has a strong technical match with this job description.");
        }
        else if (response.MatchScore >= 60)
        {
            response.Recommendations.Add(
                "Your resume has a moderate match. Highlight the most relevant experience.");
        }
        else if (requiredSkills.Count > 0)
        {
            response.Recommendations.Add(
                "Your resume is missing several skills requested by this job description.");
        }

        return Task.FromResult(response);
    }
}