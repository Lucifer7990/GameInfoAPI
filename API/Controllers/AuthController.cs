using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utilities.Email;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext dbContext, IConfiguration _config) : ControllerBase
{
    //POST : /api/auth/send-otp
    [HttpPost("send-otp")]
    public async Task<ActionResult> SendOTP(OtpSendRequest req)
    {

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

        if (user is null)
        {
            user = new User { Email = req.Email };
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
            senderEmail:"darjidhruv720@gmail.com",
            senderName: "MyApp Security"
        );

        // 2. Generate + send OTP
        string otp = "123456";
        await emailSender.SendOtpAsync(req.Email, otp, expiryMinutes: 10);

        // 3. Store otp + expiry in your DB/cache for verification later


        return Ok(new { Message = "OTP Send Successfully" });
    }

    //POST : /api/auth/verify-otp

    [HttpPost("verify-otp")]
    public async Task<ActionResult<User>> VerifyOTP(OtpVerifyRequest req)
    {

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

        if (user is null) return NotFound();

        if (user.OtpExpiryTime < DateTime.UtcNow) return Unauthorized();

        if (user.CurrentOtp.ToUpper() == req.OtpHash.ToUpper())
        {
            user.IsActive = true;

            var Key = Environment.GetEnvironmentVariable("SECRATE_KEY") ?? "YourDhruvSecretLongKeyWithAtLeast32Chars";

            Console.WriteLine(Key);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, req.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Response.Cookies.Append("authToken", tokenString, new CookieOptions
            {
                HttpOnly = true,                          // JS cannot access
                Secure = false,                          // HTTPS only
                SameSite = SameSiteMode.Lax,           // CSRF protection
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });



            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Login successful" });
        }
        else
            return BadRequest();
    }
}
