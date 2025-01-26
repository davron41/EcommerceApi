using Microsoft.AspNetCore.Mvc;
using Twewew.Requests.Auth;
using Twewew.Responses.Auth;
using Twewew.Services.Interfaces;

namespace Twewew.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _service.LoginAsync(request);

        return Ok(response);
    }
    [HttpPost("register")]
    public async Task<ActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        await _service.RegisterAsync(request);

        return NoContent();
    }

    [HttpPost("confirm-email")]
    public async Task<ActionResult> ConfirmEmailAsync(EmailConfirmationRequest request)
    {
        await _service.ConfirmEmailAsync(request);

        return NoContent();
    }
    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        await _service.ResetPasswordAsync(request);

        return NoContent();
    }
    [HttpPost("confirm-password-reset")]
    public async Task<ActionResult> ConfirmResetPasswordAsync(ConfirmResetPasswordRequest request)
    {
        await _service.ConfirmResetPasswordAsync(request);

        return NoContent();
    }
    [HttpPost("refresh-token")]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = await _service.RefreshAsync(request);

        return Ok(response);
    }
}
