namespace Application.Services
{
    public interface IProfileService
    {
        Task<ProfileDetailsDto> GetProfileDetails();
    }
}