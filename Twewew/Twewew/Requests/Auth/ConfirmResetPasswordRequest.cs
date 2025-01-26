namespace Twewew.Requests.Auth;

public record ConfirmResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword);
