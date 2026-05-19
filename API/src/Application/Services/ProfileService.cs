using System.Security.Claims;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ProfileService(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext, IMapper mapper) : IProfileService
{
    public async Task DeleteUserAsync()
    {
        var userPrincipal = httpContextAccessor.HttpContext?.User;
        var UserId = userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        var user = await dbContext.Users.Where(x => x.IsActive == true && x.Id.ToString() == UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"Active user with ID {UserId} was not found.");
        }

        user.IsActive = false;
        await dbContext.SaveChangesAsync();
    }

    public async Task<ProfileDetailsDto> GetProfileDetailAsync()
    {
        var userPrincipal = httpContextAccessor.HttpContext?.User;
        var UserId = userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        var user = await dbContext.Users.Where(x => x.IsActive == true && x.Id.ToString() == UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"Active user with ID {UserId} was not found.");
        }

        return mapper.Map<ProfileDetailsDto>(user);
    }

    public async Task UpdateUserAsync(UserUpdateDto userDto)
    {
        var userPrincipal = httpContextAccessor.HttpContext?.User;
        var UserId = userPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(UserId))
        {
            throw new UnauthorizedAccessException("User is not authenticated or ID is invalid.");
        }

        var user = await dbContext.Users.Where(x => x.IsActive == true && x.Id.ToString() == UserId).FirstOrDefaultAsync();

        if (user == null)
        {
            throw new KeyNotFoundException($"Active user with ID {UserId} was not found.");
        }

        mapper.Map(userDto, user);

        await dbContext.SaveChangesAsync();
    }
}