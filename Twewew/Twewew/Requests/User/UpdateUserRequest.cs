namespace Twewew.Requests.User;

public record UpdateUserRequest(
    Guid Id,
    string UserName,
    string Email,
    string Role,
    string? PhoneNumber);
