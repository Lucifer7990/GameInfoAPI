using Microsoft.EntityFrameworkCore;

namespace Application.Services;

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
            int expiryMinutes = 3600;

            user.CurrentOtp = otp.Hash(OTP);
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(expiryMinutes);

            await dbContext.SaveChangesAsync();

            await emailSender.SendOtpAsync(Email, OTP, expiryMinutes);

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

                var tokenString = token.GenerateUserToken(email: Email,username : user.Username,identity: user.Id.ToString());

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