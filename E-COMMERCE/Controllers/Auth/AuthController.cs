using Microsoft.AspNetCore.Mvc;
using ECommerce.Data;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;
using ECommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly ECommerceDbContext _dbContext;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailService _emailService;

        public AuthController(
            JwtService jwtService,
            ECommerceDbContext dbContext,
            PasswordHasher<User> passwordHasher,
            IWebHostEnvironment environment,
            IEmailService emailService)
        {
            _jwtService = jwtService;
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _environment = environment;
            _emailService = emailService;
        }

        // POST: api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _jwtService.Authenticate(request.Username, request.Password);
            if (result == null)
            {
                return Unauthorized("Invalid credentials");
            }

            return Ok(result);
        }

        // POST: api/auth/seed (temporary)
        [AllowAnonymous]
        [HttpPost("seed")]
        public async Task<ActionResult> SeedUser([FromBody] SeedUserRequest request)
        {
            if (!_environment.IsDevelopment())
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and password are required");
            }

            var exists = await _dbContext.Users.AnyAsync(u => u.Username == request.Username);
            if (exists)
            {
                return Conflict("User already exists");
            }

            var user = new User
            {
                Username = request.Username,
                Role = string.IsNullOrWhiteSpace(request.Role) ? "Admin" : request.Role,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "User created", user.Username, user.Role });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == request.Email);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var token = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(64));

            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.Now.AddHours(1);
            await _dbContext.SaveChangesAsync();

            // Replace with your frontend URL
            var resetLink = $"http://localhost:5173/reset-password?token={token}";

            var emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; }}
        .container {{ max-width: 600px; margin: 20px auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden; font-family: Arial, sans-serif; }}
        .header {{ background-color: #00C853; padding: 40px 20px; text-align: center; color: white; }}
        .header h1 {{ margin: 0; font-size: 24px; font-weight: bold; }}
        .content {{ padding: 30px 20px; background-color: #ffffff; text-align: center; }}
        .content p {{ font-size: 16px; color: #555; margin-bottom: 20px; }}
        .button {{ display: inline-block; padding: 15px 30px; background-color: #FFD600; color: #000; text-decoration: none; font-weight: bold; border-radius: 25px; font-size: 16px; margin: 20px 0; }}
        .footer {{ padding: 20px; text-align: center; font-size: 12px; color: #888; background-color: #f9f9f9; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Password Reset Request</h1>
        </div>
        <div class='content'>
            <p>Hello,</p>
            <p>We received a request to reset your password. If you didn't make this request, you can ignore this email.</p>
            <a href='{resetLink}' class='button'>RESET PASSWORD</a>
            <p>Or copy and paste this link into your browser:</p>
            <p style='word-break: break-all; color: #00C853;'><small>{resetLink}</small></p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply.</p>
            <p>&copy; {DateTime.Now.Year} E-Commerce App</p>
        </div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(request.Email, "Reset Password", emailBody);

            return Ok(new { message = "OK" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid or expired token.");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _dbContext.SaveChangesAsync();

            return Ok(new { message = "Password reset successfully." });
        }
    }
}
