using AutoMapper;
using Domain.Entities;
using Application.DTO;
using Application.UseCases.Comands;
using Npgsql.TypeMapping;
using Application.UseCases.Queries;

namespace Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<RegisterDTO, UserRegisterCommand>();
            CreateMap<int, UserLogoutCommand>()
                .ForMember(c=>c.UserId, opt=>opt.MapFrom(src=>src));

            CreateMap<RefreshTokenDTO, RefreshTokenCommand>();
            CreateMap<int, GetUserQueryByIdQuery>()
                .ForMember(c => c.Id, opt => opt.MapFrom(src => src));

            CreateMap<(int,int),DeleteUserCommand>()
                .ForMember(c=>c.Id, opt=>opt.MapFrom(src=>src.Item1))
                .ForMember(c=>c.InitiatorId, opt=>opt.MapFrom(src=>src.Item2));

            CreateMap<(RegisterDTO,int),UpdateUserCommand>()
                .ForMember(c=>c.user, opt=>opt.MapFrom(src=>src.Item1))
                .ForMember(c=>c.id, opt=>opt.MapFrom(src=>src.Item2));
        }
    }
}
