namespace AIResume.Domain.Entities;

public class Resume
{
    public Resume(
        Guid userId,
        string fileName,
        string filePath)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        FileName = fileName;
        FilePath = filePath;
        UploadedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string FileName { get; private set; }

    public string FilePath { get; private set; }

    public DateTime UploadedAtUtc { get; private set; }
}