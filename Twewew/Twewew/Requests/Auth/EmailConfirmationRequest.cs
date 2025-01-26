namespace Twewew.Requests.Auth;

public record EmailConfirmationRequest
    (string Email,
    string Token);
