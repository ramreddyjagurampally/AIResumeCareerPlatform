using System.Text;
using AIResume.Application.Resumes.Interfaces;
using UglyToad.PdfPig;

namespace AIResume.Infrastructure.Parsing;

public class PdfResumeParserService : IResumeParserService
{
    public Task<string> ExtractTextAsync(string filePath)
    {
        var text = new StringBuilder();

        using var document = PdfDocument.Open(filePath);

        foreach (var page in document.GetPages())
        {
            text.AppendLine(page.Text);
        }

        return Task.FromResult(text.ToString());
    }
}