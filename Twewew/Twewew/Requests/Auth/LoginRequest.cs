namespace Twewew.Requests.Auth;

public record LoginRequest(
    string UserName,
    string Password);
