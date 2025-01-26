namespace Twewew.DTOs;

public class UserDto
{
    public Guid UserId { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public string? Role { get; set; }
    public string? PhoneNumber { get; set; }
}
