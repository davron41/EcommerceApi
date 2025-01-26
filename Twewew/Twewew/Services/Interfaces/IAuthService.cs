using Twewew.Requests.Auth;
using Twewew.Responses.Auth;

namespace Twewew.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshAsync(RefreshTokenRequest request);
    Task RegisterAsync(RegisterRequest request);
    Task ConfirmEmailAsync(EmailConfirmationRequest request);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task ConfirmResetPasswordAsync(ConfirmResetPasswordRequest request);


}
