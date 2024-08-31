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
            CreateMap<LoginDTO,UserLoginCommand>();
            CreateMap<int, UserLogoutCommand>();
            CreateMap<RefreshTokenDTO, RefreshTokenCommand>();
            CreateMap<int, GetUserQueryByIdQuery>();
            CreateMap<(int,int),DeleteUserCommand>();
            CreateMap<(RegisterDTO,int),UpdateUserCommand>();
        }
    }
}
