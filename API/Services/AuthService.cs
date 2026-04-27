using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameInfoAPI.API.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Utilities.Email;

namespace API.Services;

public class AuthService(AppDbContext dbContext, IConfiguration _config) : IAuthService
{
    async Task<bool> IAuthService.SendOTP(string Email)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user is null)
            {
                user = new User { Email = Email };
                dbContext.Users.Add(user);
                // await dbContext.SaveChangesAsync();
            }


            string OTP = "1234";
            byte[] sourceBytes = Encoding.UTF8.GetBytes(OTP);

            // Use the static 'HashData' method for a quick, one-line hash
            byte[] hashBytes = SHA256.HashData(sourceBytes);

            // Convert byte array to a readable Hex string
            string hash = Convert.ToHexString(hashBytes);

            user.CurrentOtp = hash;
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);


            await dbContext.SaveChangesAsync();

            // 1. Set up the sender (do this once, e.g. in DI)
            var emailSender = new OtpEmailSender(
                smtpHost: "smtp.gmail.com",
                smtpPort: 587,
                username: "darjidhruv720@gmail.com",
                password: "pxis ndnp hnen vcoe",
                senderEmail: "darjidhruv720@gmail.com",
                senderName: "MyApp Security"
            );

            // 2. Generate + send OTP
            string otp = "123456";
            await emailSender.SendOtpAsync(Email, otp, expiryMinutes: 10);

            // 3. Store otp + expiry in your DB/cache for verification later


            return true;
        }
        catch
        {
            return false;
        }
    }

    async Task<string?> IAuthService.VerifyOTP(string Email, string OTP)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);

            if (user is null) return null;

            if (user.OtpExpiryTime < DateTime.UtcNow) return null;

            if (user.CurrentOtp.ToUpper() == OTP.ToUpper())
            {
                user.IsActive = true;

                var Key = Environment.GetEnvironmentVariable("SECRATE_KEY") ?? "YourDhruvSecretLongKeyWithAtLeast32Chars";

                Console.WriteLine(Key);
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, Email),
                new Claim(ClaimTypes.Role, "User")
            };

                var token = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: credentials);

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                await dbContext.SaveChangesAsync();
                return tokenString;
            }
            else
                return null;
        }
        catch
        {
            return null;
        }
    }
}