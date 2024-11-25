using Authorization.Application.UseCases.Commands.Create;
using Authorization.Core.Entities;
using AutoMapper;

namespace Authorization.Infrastructure.Mapping;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterUserRequest, UserEntity>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Entity.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Entity.Firstname))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Entity.Lastname))
            .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
    }
    
}