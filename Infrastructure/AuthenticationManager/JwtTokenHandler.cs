using AuthenticationManager.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationManager
{
    public class JwtTokenHandler
    {
        private readonly IConfiguration configuration;
        public JwtTokenHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //public async Task<JwtSecurityToken> GenerateToken(JwtAuthRequest user, List<int> routeIds)
        public async Task<(JwtSecurityToken AccessToken, string RefreshToken, DateTime RefreshTokenExpiry)> GenerateToken(JwtAuthRequest user, List<int> routeIds)
        {
            try
            {
                var defaultClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString()),
                    new Claim("UserID",user.Id.ToString()),
                    new Claim("SecurityKey",user.SecurityKey),
                    new Claim("RoleTypeCode",user.RoleTypeCode.ToString()), 
                    new Claim("Role",user.Role)
                };
                if (routeIds != null && routeIds.Any())
                {
                    foreach (var routeId in routeIds)
                    {
                        defaultClaims.Add(new Claim("RouteId", routeId.ToString()));
                    }
                }
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
                var jwtSecurityToken = new JwtSecurityToken(
                       issuer: configuration["JWT:ValidIssuer"],
                       audience: configuration["JWT:ValidAudience"],
                       expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["JWT:DurationInMinutes"])),
                       claims: defaultClaims,
                       signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                       );
                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                return (jwtSecurityToken, refreshToken, refreshTokenExpiry);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
