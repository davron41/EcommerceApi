namespace Twewew.Requests.Auth;

public record ResetPasswordRequest(
    string Email,
    string RedirectUrl,
    string OS,
    string Browser);
