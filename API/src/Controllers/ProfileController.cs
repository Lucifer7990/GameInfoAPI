
using System.Security.Claims;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfileController(IProfileService profile) : ControllerBase
{

    [HttpGet("me")]
    [Authorize(Roles = "User")]
    public async Task<ProfileDetailsDto> GetProfile()
    {
        return await profile.GetProfileDetailAsync();
    }

    [HttpPut("me")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> UpdateProfile(UserUpdateDto userdto)
    {
        await profile.UpdateUserAsync(userdto);
        return NoContent();
    }

    [HttpDelete("me")]
    [Authorize(Roles = "User")]
    public async Task<object> DelateProfile()
    {
        await profile.DeleteUserAsync();

        Response.Cookies.Delete("AuthToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddDays(-1) // Set expiration to the past
        });

        return NoContent();
    }


}

