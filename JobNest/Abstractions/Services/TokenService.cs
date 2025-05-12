using JobNest.Abstractions.Contracts;
using JobNest.Abstractions.Services;
using JobNest.Dtos.Auth.Requests;
using JobNest.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobNest.Abstractions.Services
{
   
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContext;

        public TokenService(IOptions<JwtSettings> jwtOptions, IHttpContextAccessor httpContext)
        {
            _jwtSettings = jwtOptions.Value;
            _httpContext = httpContext;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public CurrentUser GetToken()
        {
            var accessToken = _httpContext.HttpContext?.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(accessToken) || !accessToken.StartsWith("Bearer "))
                return null;

            var tokenStr = accessToken.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(tokenStr);

            return new CurrentUser
            {
                UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
                Email = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email)?.Value,
                UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value,
            };
        }
    }
}

