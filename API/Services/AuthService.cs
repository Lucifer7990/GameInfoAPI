using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameInfoAPI.API.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class AuthService(AppDbContext dbContext, IMessageSender emailSender, IOtpService otp,ITokenService token) : IAuthService
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

            string OTP = otp.Generate();

            user.CurrentOtp = otp.Hash(OTP);
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);

            await dbContext.SaveChangesAsync();

            await emailSender.SendOtpAsync(Email, OTP, expiryMinutes: 10);

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

                var tokenString = token.GenerateJwt(email: Email, role: "User");

                user.IsActive = true;

                
                

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