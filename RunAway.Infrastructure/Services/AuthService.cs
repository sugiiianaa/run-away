using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RunAway.Application.IServices;
using RunAway.Domain.Enums;
using RunAway.Infrastructure.Authentication;

namespace RunAway.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;

        public AuthService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public (string token, DateTime expiresAt) GenerateToken(Guid ID, string Email, UserRoles role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddHours(_jwtSettings.DurationInHours);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, ID.ToString()),
                new(JwtRegisteredClaimNames.Email, Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, role.ToString()!),
            };


            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }
    }
}
