using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Twewew.Services.Interfaces;
using Twewew.Settings;

namespace Twewew.Services;

public sealed class TokenHandler : ITokenHandler
{
    private readonly TokenSettings _settings;

    public TokenHandler(IOptions<TokenSettings> settings)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
    }

    public string GenerateJwtToken(IdentityUser<Guid> user, IEnumerable<string> roles)
    {
        var claims = GetClaims(user, roles);
        var signingKey = GetClaimKey();
        var securityToken = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            signingCredentials: signingKey,
            expires: DateTime.UtcNow.AddHours(_settings.JwtExpiresInHours));

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
    public string GenerateRefreshToken()
    {
        var randomNumbers = new byte[32];
        using var randomGenerator = RandomNumberGenerator.Create();
        randomGenerator.GetBytes(randomNumbers);

        var token = Convert.ToBase64String(randomNumbers);

        return token;
    }

    private static List<Claim> GetClaims(IdentityUser<Guid> user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier,user.Id.ToString()),
            new(ClaimTypes.Name,user.UserName!),
            new(ClaimTypes.Email,user.Email!)
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        return claims;
    }

    private SigningCredentials GetClaimKey()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
        var signingKey = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        return signingKey;
    }


}
