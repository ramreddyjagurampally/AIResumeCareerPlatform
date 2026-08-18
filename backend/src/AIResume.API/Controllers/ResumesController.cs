using System.Security.Claims;
using AIResume.Application.Jobs.DTOs;
using AIResume.Application.Jobs.Interfaces;
using AIResume.Application.Resumes.Interfaces;
using AIResume.Application.Resumes.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIResume.API.Controllers;

[ApiController]
[Route("api/resumes")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly IResumeUploadService _resumeUploadService;
    private readonly IResumeRepository _resumeRepository;
    private readonly IResumeParserService _resumeParserService;
    private readonly IResumeAnalysisService _resumeAnalysisService;
    private readonly IJobMatchService _jobMatchService;

    public ResumesController(
        IResumeUploadService resumeUploadService,
        IResumeRepository resumeRepository,
        IResumeParserService resumeParserService,
        IResumeAnalysisService resumeAnalysisService,
        IJobMatchService jobMatchService)
    {
        _resumeUploadService = resumeUploadService;
        _resumeRepository = resumeRepository;
        _resumeParserService = resumeParserService;
        _resumeAnalysisService = resumeAnalysisService;
        _jobMatchService = jobMatchService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please select a resume file.");
        }

        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Uploads",
            "Resumes");

        Directory.CreateDirectory(uploadsFolder);

        var storedFileName =
            $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

        var filePath =
            Path.Combine(uploadsFolder, storedFileName);

        await using (var stream =
            new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var resume =
            await _resumeUploadService.UploadAsync(
                userId,
                file.FileName,
                filePath);

        return Ok(new
        {
            resume.Id,
            resume.FileName,
            resume.UploadedAtUtc
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetMyResumes()
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var resumes =
            await _resumeRepository.GetByUserIdAsync(userId);

        return Ok(resumes.Select(resume => new
        {
            resume.Id,
            resume.FileName,
            resume.UploadedAtUtc
        }));
    }

    [HttpGet("{resumeId:guid}/text")]
    public async Task<IActionResult> GetResumeText(Guid resumeId)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var resume =
            await _resumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
        {
            return NotFound("Resume not found.");
        }

        if (resume.UserId != userId)
        {
            return Forbid();
        }

        if (!System.IO.File.Exists(resume.FilePath))
        {
            return NotFound("Resume file not found.");
        }

        var extension =
            Path.GetExtension(resume.FileName)
                .ToLowerInvariant();

        if (extension != ".pdf")
        {
            return BadRequest(
                "PDF parsing is currently supported only for PDF resumes.");
        }

        var text =
            await _resumeParserService.ExtractTextAsync(
                resume.FilePath);

        return Ok(new
        {
            resume.Id,
            resume.FileName,
            Text = text
        });
    }

    [HttpGet("{resumeId:guid}/analyze")]
    public async Task<IActionResult> AnalyzeResume(Guid resumeId)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        var resume =
            await _resumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
        {
            return NotFound("Resume not found.");
        }

        if (resume.UserId != userId)
        {
            return Forbid();
        }

        if (!System.IO.File.Exists(resume.FilePath))
        {
            return NotFound("Resume file not found.");
        }

        var extension =
            Path.GetExtension(resume.FileName)
                .ToLowerInvariant();

        if (extension != ".pdf")
        {
            return BadRequest(
                "Resume analysis currently supports PDF files only.");
        }

        var resumeText =
            await _resumeParserService.ExtractTextAsync(
                resume.FilePath);

        var analysis =
            await _resumeAnalysisService.AnalyzeAsync(
                resumeText);

        return Ok(analysis);
    }

    [HttpPost("{resumeId:guid}/match-job")]
    public async Task<IActionResult> MatchJob(
        Guid resumeId,
        JobMatchRequest request)
    {
        var userIdValue =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst("sub")?.Value;

        if (!Guid.TryParse(userIdValue, out var userId))
        {
            return Unauthorized();
        }

        if (string.IsNullOrWhiteSpace(request.JobDescription))
        {
            return BadRequest("Job description is required.");
        }

        var resume =
            await _resumeRepository.GetByIdAsync(resumeId);

        if (resume == null)
        {
            return NotFound("Resume not found.");
        }

        if (resume.UserId != userId)
        {
            return Forbid();
        }

        if (!System.IO.File.Exists(resume.FilePath))
        {
            return NotFound("Resume file not found.");
        }

        var extension =
            Path.GetExtension(resume.FileName)
                .ToLowerInvariant();

        if (extension != ".pdf")
        {
            return BadRequest(
                "Job matching currently supports PDF resumes only.");
        }

        var resumeText =
            await _resumeParserService.ExtractTextAsync(
                resume.FilePath);

        var result =
            await _jobMatchService.MatchAsync(
                resumeText,
                request.JobDescription);

        return Ok(result);
    }
}