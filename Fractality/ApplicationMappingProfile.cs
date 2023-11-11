using AutoMapper;
using Fractality.Models.Backend;
using Fractality.Models.Frontend;

namespace Fractality
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile() 
        {
            CreateMap<User, UserModel>().ReverseMap();
            CreateMap<Fractal, FractalModel>().ReverseMap();
        }
    }
}
