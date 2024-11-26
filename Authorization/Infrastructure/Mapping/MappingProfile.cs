using Authorization.Application.UseCases.Commands.Create;
using Authorization.Core.DTOs;
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
        CreateMap<UserEntity, UserDto>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src=>src.PasswordEntity.Password));
        CreateMap<UserDto, GetUserDto>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
    
}