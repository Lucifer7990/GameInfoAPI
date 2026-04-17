using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext dbContext) : ControllerBase
{
    //POST : /api/auth/send-otp
    [HttpPost("send-otp")]
    public async Task<ActionResult<User>> SendOTP(OtpSendRequest req)
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


        return Ok(await dbContext.Users.ToListAsync());
    }

    //POST : /api/auth/verify-otp

    [HttpPost("verify-otp")]
    public async Task<ActionResult<User>> VerifyOTP(OtpVerifyRequest req)
    {

        var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == req.Email);

        if (user is null) return NotFound();

        if (user.OtpExpiryTime < DateTime.UtcNow) return Unauthorized();

        if(user.CurrentOtp.ToUpper()==req.OtpHash.ToUpper())
        {
            user.IsActive=true;
        }
        else
            return BadRequest();

        await dbContext.SaveChangesAsync();

        return Ok("OTP Varified successfully");
    }
}
