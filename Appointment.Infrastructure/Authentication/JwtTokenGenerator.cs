
using System.Text;
using Appointment.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Jwt = System.IdentityModel.Tokens.Jwt;
// Keep this for the TokenHandler and SecurityToken
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace Appointment.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions  _jwtOptions;
    
    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(string userId, string userName, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(Jwt.JwtRegisteredClaimNames.Sub, userId),
            new Claim(Jwt.JwtRegisteredClaimNames.UniqueName, userName),
            new Claim(Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));    
        }

        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryTime),
            signingCredentials: creds

        );
        return new Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
