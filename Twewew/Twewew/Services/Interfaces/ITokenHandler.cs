using Microsoft.AspNetCore.Identity;

namespace Twewew.Services.Interfaces;

public interface ITokenHandler
{
    string GenerateJwtToken(IdentityUser<Guid> user, IEnumerable<string> roles);
    string GenerateRefreshToken();

}
