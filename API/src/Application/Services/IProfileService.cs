namespace Application.Services
{
    public interface IProfileService
    {
        Task<ProfileDetailsDto> GetProfileDetailAsync();
        Task UpdateUserAsync(UserUpdateDto userDto);

        Task DeleteUserAsync();
    }
}