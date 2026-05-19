using Application.Services;
using Microsoft.AspNetCore.Mvc;


namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("send-otp")]
    public async Task<ActionResult<AuthResponse>> SendOTP(OtpSendRequest req)
    {
        var success = await auth.SendOTP(req.Email);

        if (!success)
            return BadRequest("OTP sending failed");

        return Ok(new AuthResponse("OTP sent successfully"));
    }

    [HttpPost("verify-otp")]
    public async Task<ActionResult<AuthResponse>> VerifyOTP(OtpVerifyRequest req)
    {
        var token = await auth.VerifyOTP(req.Email, req.OtpHash);

        if (token is null)
            return Unauthorized("Invalid or expired OTP");

        Response.Cookies.Append("authToken", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
        });

        return Ok(new AuthResponse("Login successful"));
    }
}