using AutoMapper;

namespace Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map from Entity to DTO (For reading data)
            CreateMap<User, ProfileDetailsDto>();

            // Map from DTO to Entity (For creating/saving data)
            CreateMap<ProfileDetailsDto, User>();
            CreateMap<UserUpdateDto, User>();
            

        }
    }
}