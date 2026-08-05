namespace AIResume.Application.Users.DTOs;
public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; }=string.Empty;
    public string Email { get; set; }=string.Empty;
    public DateTime CreatedAtUtc { get; set; }
}