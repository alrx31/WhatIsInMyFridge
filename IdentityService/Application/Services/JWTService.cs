using Infastructure.Middlewares.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IJWTService
    {
        string GenerateJwtToken(string email);
        string GenerateRefreshToken();
        ClaimsPrincipal? GetTokenPrincipal(string jwtToken);
    }


    public class JWTService : IJWTService
    {
        private readonly string _key;

        public JWTService(IConfiguration config)
        {
            _key = config["Jwt:Key"];
        }

        public string GenerateJwtToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }
            
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetTokenPrincipal(string jwtToken)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ClockSkew = TimeSpan.Zero // set thats for testing
            };

            try
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out _);
                
                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new ValidationDataException("Security token has expired.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                
                return null;
            }
        }
    }
}
