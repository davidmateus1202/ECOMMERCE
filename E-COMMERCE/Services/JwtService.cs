using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace ECommerce.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ECommerceDbContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        // Constructor
        public JwtService(IConfiguration configuration, ECommerceDbContext dbContext, PasswordHasher<User> passwordHasher)
        {
            _configuration = configuration;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> Authenticate(
            string username,
            string passwordHash
        )
        {
            // validate information
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(passwordHash))
            {
                return null;
            }

            // validate user in database
            var userAccount = await _dbContext.Users.FirstOrDefaultAsync(p => p.Username == username);
            if (userAccount == null || _passwordHasher.VerifyHashedPassword(userAccount, userAccount.PasswordHash, passwordHash) == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var key = _configuration["JwtConfig:key"];
            var tokenValidityMins = _configuration.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokenExpiry = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.NameId, userAccount.Id.ToString()),
                }),
                Expires = tokenExpiry,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            userAccount.AccessToken = accessToken;
            userAccount.ExpiresIn = tokenValidityMins;
            await _dbContext.SaveChangesAsync();

            return new User
            {
                Id = userAccount.Id,
                Username = userAccount.Username,
                Role = userAccount.Role,
                AccessToken = accessToken,
                ExpiresIn = tokenValidityMins
            };
        }
    }
}