
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
        return await profile.GetProfileDetails();
    }

    [HttpPatch("me")]
    [Authorize(Roles = "User")]
    public async Task<object> UpdateProfile()
    {
        var result = new {  nameid = User.FindFirstValue(ClaimTypes.NameIdentifier), 
                            name = User.FindFirstValue(ClaimTypes.Name),
                            email =  User.FindFirstValue(ClaimTypes.Email),
                            Role =  User.FindFirstValue(ClaimTypes.Role),
                            temp =  User.FindFirstValue(ClaimTypes.GivenName)
                         };
        return result;
    }

    [HttpDelete("me")]
    [Authorize(Roles = "User")]
    public async Task<object> DelateProfile()
    {
        var result = new {  nameid = User.FindFirstValue(ClaimTypes.NameIdentifier), 
                            name = User.FindFirstValue(ClaimTypes.Name),
                            email =  User.FindFirstValue(ClaimTypes.Email),
                            Role =  User.FindFirstValue(ClaimTypes.Role),
                            temp =  User.FindFirstValue(ClaimTypes.GivenName)
                         };
        return result;
    }
   
   
}
