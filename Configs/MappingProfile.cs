using AutoMapper;
using AuthAPI.models;
using AuthAPI.models.request;
using AuthAPI.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, RegisterResDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore());
    }
}
