namespace Twewew.Responses.Auth;

public record LoginResponse(
    string AccessToken,
    string RefreshToken);
