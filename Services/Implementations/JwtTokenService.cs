using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Como.CRM.Api.Domain.Entities;
using Como.CRM.Api.DTOs.Auth;
using Como.CRM.Api.Options;
using Como.CRM.Api.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Como.CRM.Api.Services.Implementations;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public LoginResponse CreateToken(Tenant tenant, AppUser user, List<string> permissions)
    {
        var expiresAt = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName),
            new("tenant_id", tenant.PublicId.ToString()),
            new("tenant_host", tenant.Host)
        };

        claims.AddRange(permissions.Select(p => new Claim("permission", p)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpirationMinutes = _options.ExpirationMinutes,
            ExpiresAtUtc = expiresAt,
            Permissions = permissions
        };
    }
}
