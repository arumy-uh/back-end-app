using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EctoTec.Models; 

namespace EctoTec.Custom
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;

        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string generateJWT(User userN)
        {
            var userCLaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userN.UserName.ToString()),
                new Claim(ClaimTypes.Email, userN.Email!)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));
            var credeentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);

            var jwtConfiguration = new JwtSecurityToken(
                claims: userCLaims,
                signingCredentials: credeentials,
                expires: DateTime.UtcNow.AddMinutes(25)
            );
            return new JwtSecurityTokenHandler().WriteToken(jwtConfiguration);
        }
        public bool validateToken(string token)
        {
            var claims = new ClaimsPrincipal();
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationsParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!)),
            };
            try
            {
                claims = tokenHandler.ValidateToken(token,validationsParameter, out SecurityToken validationToken);
                return true;
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
