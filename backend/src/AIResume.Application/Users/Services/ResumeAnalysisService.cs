using AIResume.Application.Resumes.DTOs;
using AIResume.Application.Resumes.Interfaces;

namespace AIResume.Application.Resumes.Services;

public class ResumeAnalysisService : IResumeAnalysisService
{
    public Task<ResumeAnalysisResponse> AnalyzeAsync(string resumeText)
    {
        var response = new ResumeAnalysisResponse();

        if (string.IsNullOrWhiteSpace(resumeText))
        {
            response.AtsScore = 0;
            response.MissingSections.Add("Resume Content");
            response.Suggestions.Add(
                "Resume text could not be analyzed. Upload a readable PDF resume.");

            return Task.FromResult(response);
        }

        var text = resumeText.ToLowerInvariant();

        var knownSkills = new[]
        {
            "c#",
            ".net",
            "asp.net core",
            "asp.net",
            "react",
            "typescript",
            "javascript",
            "python",
            "java",
            "sql",
            "sql server",
            "mysql",
            "postgresql",
            "azure",
            "aws",
            "docker",
            "kubernetes",
            "entity framework",
            "entity framework core",
            "git",
            "github",
            "rest api",
            "web api",
            "microservices",
            "html",
            "css",
            "node.js",
            "angular"
        };

        foreach (var skill in knownSkills)
        {
            if (text.Contains(skill) &&
                !response.Skills.Contains(skill))
            {
                response.Skills.Add(skill);
            }
        }

        // Professional Summary
        if (ContainsAny(
                text,
                "professional summary",
                "summary",
                "career summary",
                "profile"))
        {
            response.Strengths.Add(
                "Professional summary section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Professional Summary");

            response.Suggestions.Add(
                "Add a concise professional summary highlighting your experience, strongest technical skills, and target role.");
        }

        // Experience
        if (ContainsAny(
                text,
                "experience",
                "work experience",
                "professional experience",
                "employment"))
        {
            response.Strengths.Add(
                "Experience section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Experience");

            response.Suggestions.Add(
                "Add a work experience section with job titles, employers, dates, responsibilities, and measurable achievements.");
        }

        // Education
        if (ContainsAny(
                text,
                "education",
                "academic background"))
        {
            response.Strengths.Add(
                "Education section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Education");

            response.Suggestions.Add(
                "Add an education section with degree, university, location, and graduation date.");
        }

        // Skills
        if (ContainsAny(
                text,
                "skills",
                "technical skills",
                "technologies",
                "core competencies"))
        {
            response.Strengths.Add(
                "Skills section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Skills");

            response.Suggestions.Add(
                "Add a dedicated technical skills section to improve ATS keyword matching.");
        }

        // Projects
        if (ContainsAny(
                text,
                "projects",
                "project experience",
                "academic projects",
                "personal projects"))
        {
            response.Strengths.Add(
                "Projects section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Projects");

            response.Suggestions.Add(
                "Add relevant technical projects that demonstrate your hands-on experience.");
        }

        // Certifications
        if (ContainsAny(
                text,
                "certifications",
                "certification",
                "certificates"))
        {
            response.Strengths.Add(
                "Certifications section detected.");
        }
        else
        {
            response.MissingSections.Add(
                "Certifications");

            response.Suggestions.Add(
                "Consider adding relevant certifications if you have any.");
        }

        // Contact / professional links
        if (!text.Contains("linkedin"))
        {
            response.Suggestions.Add(
                "Consider adding your LinkedIn profile.");
        }

        if (!text.Contains("github"))
        {
            response.Suggestions.Add(
                "Consider adding your GitHub profile, especially for software engineering roles.");
        }

        // Skill volume
        if (response.Skills.Count < 5)
        {
            response.Suggestions.Add(
                "Add more role-relevant technical skills and keywords.");
        }
        else
        {
            response.Strengths.Add(
                $"{response.Skills.Count} relevant technical skills detected.");
        }

        // Measurable achievements
        var containsNumbers =
            resumeText.Any(char.IsDigit);

        var containsPercentage =
            resumeText.Contains("%");

        if (!containsNumbers && !containsPercentage)
        {
            response.Suggestions.Add(
                "Add measurable achievements using numbers, percentages, performance improvements, cost savings, or user impact.");
        }
        else
        {
            response.Strengths.Add(
                "Quantifiable information detected.");
        }

        // Resume length
        if (resumeText.Length < 1200)
        {
            response.Suggestions.Add(
                "The resume appears brief. Add more detail about your experience, projects, responsibilities, and accomplishments.");
        }

        // Action verbs
        var actionVerbs = new[]
        {
            "developed",
            "built",
            "implemented",
            "designed",
            "created",
            "optimized",
            "improved",
            "deployed",
            "automated",
            "integrated",
            "led"
        };

        var actionVerbCount =
            actionVerbs.Count(text.Contains);

        if (actionVerbCount < 3)
        {
            response.Suggestions.Add(
                "Use stronger action verbs such as Developed, Implemented, Built, Optimized, Deployed, or Automated.");
        }
        else
        {
            response.Strengths.Add(
                "Strong action-oriented wording detected.");
        }

        // Calculate ATS score
        var score = 35;

        score += Math.Min(
            response.Skills.Count * 3,
            30);

        score += Math.Min(
            response.Strengths.Count * 5,
            30);

        score -=
            response.MissingSections.Count * 5;

        if (containsNumbers || containsPercentage)
        {
            score += 5;
        }

        if (text.Contains("linkedin"))
        {
            score += 2;
        }

        if (text.Contains("github"))
        {
            score += 2;
        }

        response.AtsScore =
            Math.Clamp(score, 0, 100);

        // Always provide at least one suggestion
        if (response.Suggestions.Count == 0)
        {
            response.Suggestions.Add(
                "Your resume includes the major sections. Tailor the skills and achievements to each job description for stronger ATS matching.");
        }

        return Task.FromResult(response);
    }

    private static bool ContainsAny(
        string text,
        params string[] keywords)
    {
        return keywords.Any(text.Contains);
    }
}