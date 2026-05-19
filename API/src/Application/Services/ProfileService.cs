using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProfileService(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext, IMapper mapper) : IProfileService
{
    public async Task<ProfileDetailsDto> GetProfileDetails()
    {
        var userPrincipal = httpContextAccessor.HttpContext?.User;
        var UserId = userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        var user = await dbContext.Users.Where(x => x.IsActive == true).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"Active user with ID {UserId} was not found.");
        }

        return mapper.Map<ProfileDetailsDto>(user);
    }

}