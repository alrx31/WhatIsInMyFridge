using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ListProfile:Profile
    {
        public ListProfile()
        {
            CreateMap<AddListDTO, AddListComand>();
            CreateMap<AddListComand, ProductsList>()
                .ForMember(dest => dest.CreateData, opt => opt.MapFrom(src => System.DateTime.UtcNow));
            CreateMap<int, GetListQuery>();
            CreateMap<string, GetListByNameQuery>();
            CreateMap<int, DeleteListComand>();
            CreateMap<(AddListDTO, string), UpdateListComand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Item1.Name))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Item1.Weight))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Item1.Price));

            CreateMap<UpdateListComand, ProductsList>();
        }

    }
}
