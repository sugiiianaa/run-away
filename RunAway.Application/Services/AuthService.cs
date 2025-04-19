using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RunAway.Application.IServices;

namespace RunAway.Application.Services
{
    public class AuthService : IAuthService
    {
        public (string token, DateTime expiresAt) GenerateToken(Guid ID, string Email)
        {
            var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ??
              throw new InvalidOperationException("JWT Key is not configured");
            var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ??
                throw new InvalidOperationException("JWT Issuer is not configured");
            var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ??
                throw new InvalidOperationException("JWT Audience is not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var expiresAt = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }
}
