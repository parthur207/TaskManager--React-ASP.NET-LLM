using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Core.Enums;
using TaskManager.Core.Ports.Security;

namespace TaskManager.Adapters.Auth
{
    public class JwtGenerator : IJwtGeneratorPort
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expirationMinutes;

        public JwtGenerator(IConfiguration configuration)
        {
            _secretKey = configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado.");
            _issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer não configurado.");
            _audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience não configurado.");

            _expirationMinutes = int.TryParse(configuration["Jwt:ExpirationInMinutes"], out var minutes)
                ? minutes
                : 60;
        }

        public string GenerateToken(Guid userId, string email, RoleUserEnum role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}