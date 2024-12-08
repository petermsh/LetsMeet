using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LetsMeet.Application.Common;
using LetsMeet.Domain.Entities;
using LetsMeet.Infrastructure.Options;
using Microsoft.IdentityModel.Tokens;

namespace LetsMeet.Infrastructure.Services.Auth;

internal class TokenService(JwtSettings settings) : ITokenService
{
    public string CreateAccessToken(AppUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("id", user.Id.ToString()),
            new("username", user.UserName!),
        };
        var expires = DateTime.UtcNow.Add(settings.Expiry);

        var token = new JwtSecurityToken(settings.Issuer,
            settings.Issuer,
            claims,
            expires: expires,
            signingCredentials: credentials);

        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenValue;
    }
}