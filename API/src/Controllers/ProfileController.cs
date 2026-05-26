
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
    public async Task<IActionResult> DelateProfile()
    {
        await profile.DeleteUserAsync();

        Response.Cookies.Delete("authToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
        });

        return NoContent();
    }

}

