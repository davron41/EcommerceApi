using System.Security.Claims;
using Twewew.Services.Interfaces;

namespace Twewew.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }
    public Guid GetUserId()
    {
        var user = _contextAccessor.HttpContext?.User ?? throw new InvalidOperationException("Current HTTP  context does not contain a user.");

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userId, out var result))
        {
            return result;
        }

        throw new InvalidOperationException($"Invalid user ID format:{userId}");
    }

    public string GetUserName()
    {
        var user = _contextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException($"Unable to get user info from HttpContext");

        if (user.Identity is null || !user.Identity.IsAuthenticated)
        {
            return string.Empty;
        }

        var userName = user.FindFirst(ClaimTypes.Name)?.Value
            ?? throw new InvalidOperationException($"User does not have name claim");

        return userName;
    }
}
