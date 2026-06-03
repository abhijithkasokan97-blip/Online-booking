
using System.Text;
using Appointment.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Jwt = System.IdentityModel.Tokens.Jwt;
// Keep this for the TokenHandler and SecurityToken
using System.IdentityModel.Tokens.Jwt;

namespace Appointment.Infrastructure.Authentication;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _config;
    public JwtTokenGenerator(IConfiguration config)
    {
        _config = config;
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

        var key =  new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer:_config["Jwt:Issuer"],
            audience: _config["Jwt:audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:expiryTime"])),
            signingCredentials: creds

        );
        return new Jwt.JwtSecurityTokenHandler().WriteToken(token);
    }
}
