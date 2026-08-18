namespace AIResume.Application.Resumes.DTOs;

public class ResumeAnalysisResponse
{
    public int AtsScore { get; set; }

    public List<string> Skills { get; set; } = new();

    public List<string> Strengths { get; set; } = new();

    public List<string> MissingSections { get; set; } = new();

    public List<string> Suggestions { get; set; } = new();
}