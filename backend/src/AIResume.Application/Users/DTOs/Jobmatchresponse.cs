namespace AIResume.Application.Jobs.DTOs;

public class JobMatchResponse
{
    public int MatchScore { get; set; }

    public List<string> MatchedSkills { get; set; } = new();

    public List<string> MissingSkills { get; set; } = new();

    public List<string> Recommendations { get; set; } = new();
}