namespace AIResume.Application.Resumes.Interfaces;

public interface IResumeParserService
{
    Task<string> ExtractTextAsync(string filePath);
}