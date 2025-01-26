namespace Twewew.Requests.Auth;

public record RegisterRequest(
    string Username,
    string Email,
    string Password,
    string ConfirmPassword,
    string ConfirmUrl,
    string? Browser,
    string? OS,
    string? RedirectUrl);
