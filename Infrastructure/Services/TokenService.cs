using Application.Interface;
using Domain.Entitiy;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? ""),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

           
             return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string ValidateTokenAndGetUserId(string Token)
        {
            if (String.IsNullOrEmpty(Token))
            {
                return null;
            }

            var tokenhandler = new JwtSecurityTokenHandler();

            try
            {
                tokenhandler.ValidateToken(Token, new TokenValidationParameters
                {

                    ValidateIssuer = true,            // Tokenı dağıtan sunucunun (Issuer) doğrulanıp doğrulanmayacağını belirler.
                    ValidateAudience = true,          // Tokenın hangi hedef kitle/uygulama (Audience) için üretildiğinin kontrol edilmesini sağlar.
                    ValidateLifetime = true,          // Tokenın süresinin dolup dolmadığını kontrol eder; süresi geçmiş tokenları reddeder.
                    ValidateIssuerSigningKey = true,  // Tokenın imzalanırken kullanılan anahtarın (Security Key), sunucudaki anahtarla eşleşip eşleşmediğine bakar.

                    ValidIssuer = _configuration["Jwt:Issuer"],     // Sistemin kabul edeceği yayıncı adını (Örn: "marangoz-api.com") konfigürasyondan okur.
                    ValidAudience = _configuration["Jwt:Audience"], // Sistemin kabul edeceği hedef kitleyi (Örn: "marangoz-app") konfigürasyondan okur.

                    // Tokenı imzalamak ve imzayı çözmek için kullanılacak olan gizli anahtarı (Secret Key) belirler.
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty)),


                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value ?? "";

                return userId;
            }
            catch (Exception ex)  
            {
                return ex.Message;
            }
        }
    }
}

