using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Twewew.Email.Models;
using Twewew.Entities;
using Twewew.Exceptions;
using Twewew.Models;
using Twewew.Persistence;
using Twewew.Requests.Auth;
using Twewew.Responses.Auth;
using Twewew.Services.Interfaces;
using Twewew.Settings;

namespace Twewew.Services;

internal sealed class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser<Guid>> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly TokenSettings _tokenSettings;
    private readonly ITokenHandler _tokenHandler;
    private readonly IEmailService _emailService;

    public AuthService(
        ApplicationDbContext context,
        ITokenHandler tokenHandler,
        UserManager<IdentityUser<Guid>> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IOptions<TokenSettings> settings,
        IEmailService emailService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _tokenSettings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user is null || !user.EmailConfirmed || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new InvalidLoginRequestException("Invalid username or password");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _tokenHandler.GenerateJwtToken(user, roles);
        var refreshToken = _tokenHandler.GenerateRefreshToken();

        await SaveRefreshTokenAsync(user, refreshToken);

        return new LoginResponse(accessToken, refreshToken);


    }

    public async Task RegisterAsync(RegisterRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var existingUser = await _userManager.FindByNameAsync(request.Username);

        if (existingUser is not null)
        {
            throw new UsernameAlreadyExistsException($"Username:{request.Username} is already taken!");
        }

        var newUser = new IdentityUser<Guid>
        {
            UserName = request.Username,
            Email = request.Email,
            PhoneNumber = null
        };

        var createResult = await _userManager.CreateAsync(newUser, request.Password);
        if (!createResult.Succeeded)
        {
            var errors = string.Join(";", createResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"User creation failed :{errors}");
        }


        await SendConfirmationEmailAsync(newUser, request);
    }
    public async Task<RefreshTokenResponse> RefreshAsync(RefreshTokenRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (token is null || token.IsRevoked)
        {
            throw new InvalidLoginRequestException("TODO: Change exception type");
        }

        if (token.ExpiresAtUtc < DateTime.UtcNow)
        {
            token.IsRevoked = true;
            await _context.SaveChangesAsync();

            throw new InvalidLoginRequestException("TODO: Change exception type");
        }

        var roles = await _userManager.GetRolesAsync(token.User);
        var accessToken = _tokenHandler.GenerateJwtToken(token.User, roles);

        return new RefreshTokenResponse(accessToken);
    }

    public async Task ConfirmEmailAsync(EmailConfirmationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new EntityNotFoundException($"The user with email:{request.Email} is not found!");
        }

        var result = await _userManager.ConfirmEmailAsync(user, request.Token);

        if (!result.Succeeded)
        {
            var errors = string.Join(";", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Email confirmation failed:{errors}");
        }

        await SendWelcomeAsync(user);
    }

    public async Task ConfirmResetPasswordAsync(ConfirmResetPasswordRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        var user = await GetAndValidateUserAsync(request.Email);

        if (!user.EmailConfirmed)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.ConfirmEmailAsync(user, token);
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Email confirmation failed. {errors}");
        }

    }
    public async Task ResetPasswordAsync(ResetPasswordRequest request)
    {

        ArgumentNullException.ThrowIfNull(request);

        var user = await GetAndValidateUserAsync(request.Email);
        var refreshToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.UserId == user.Id && !x.IsRevoked);

        if (refreshToken is not null)
        {
            refreshToken.IsRevoked = true;

        }
        await SendResetPasswordAsync(user, request);
    }

    private async Task SaveRefreshTokenAsync(IdentityUser<Guid> user, string refreshToken)
    {
        var tokenEntity = new RefreshToken
        {
            Token = refreshToken,
            IsRevoked = false,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(_tokenSettings.RefreshExpiresInDays),
            User = user,
            UserId = user.Id
        };

        _context.RefreshTokens.Add(tokenEntity);
        await _context.SaveChangesAsync();
    }

    private async Task SendConfirmationEmailAsync(IdentityUser<Guid> user, RegisterRequest request)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var redirectUrl = GetCallBackUrl(request.ConfirmUrl!, token, request.Email);
        var userInfo = new UserInfo(request.Browser ?? "Unknown browser", request.OS ?? "Unknown Operating System");
        var emailMessage = new EmailMessage(user.Email!, user.UserName!, "EmailConfirmation", redirectUrl);

        _emailService.SendEmailConfirmation(emailMessage, userInfo);
    }

    private static string GetCallBackUrl(string clientUrl, string token, string email)
    {
        var queryParams = new Dictionary<string, string>
        {
            { "email",email},
            { "token",token}
        };

        var callBackUrl = QueryHelpers.AddQueryString(clientUrl, queryParams);

        return callBackUrl;
    }
    private async Task<IdentityUser<Guid>> GetAndValidateUserAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            throw new EntityNotFoundException($"User with email:{email} is not found!");
        }

        return user;
    }

    private async Task SendWelcomeAsync(IdentityUser<Guid> user)
    {
        var emailMessage = new EmailMessage(user.Email!, user.UserName!, "Welcome", null);

        _emailService.SendWelcome(emailMessage);
    }
    private async Task SendResetPasswordAsync(IdentityUser<Guid> user, ResetPasswordRequest request)
    {
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var redirectUrl = $"{request.RedirectUrl}?token={token}&email={request.Email}";

        var emailMessage = new EmailMessage(user.Email!, user.UserName!, "PasswordReset", redirectUrl);
        var userInfo = new UserInfo(request.Browser, request.OS);

        _emailService.SendResetPassword(emailMessage, userInfo);
    }
}
