using GameInfoAPI.API.Abstractions;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService auth) : ControllerBase
{
    //POST : /api/auth/send-otp
    [HttpPost("send-otp")]
    public async Task<ActionResult> SendOTP(OtpSendRequest req)
    {

        if (await auth.SendOTP(req.Email))
        {
            return Ok(new { Message = "OTP Send Successfully" });
        }

        return Problem(title: "Authentication Process Fail", detail: "Email validation and otp send process failed due to some reasons.");
    }

    //POST : /api/auth/verify-otp

    [HttpPost("verify-otp")]
    public async Task<ActionResult<User>> VerifyOTP(OtpVerifyRequest req)
    {
        var token = await auth.VerifyOTP(req.Email, req.OtpHash);
        if (token != null)
        {
            Response.Cookies.Append("authToken", token, new CookieOptions
            {
                HttpOnly = true,                          // JS cannot access
                Secure = false,                          // HTTPS only
                SameSite = SameSiteMode.Lax,           // CSRF protection
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            return Ok(new { Message = "Lodin success" });
        }

        return Problem(title: "Authentication Process Fail", detail: "OTP Validation process failed or otp is invalid/expierd or User not found or ");

    }
}
