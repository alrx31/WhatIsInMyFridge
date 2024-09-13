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
            CreateMap<AddListDTO, AddListComand>()
                .ConstructUsing(src => new AddListComand(src));

            CreateMap<AddListDTO,ProductsList>();

            CreateMap<AddListComand, ProductsList>()
                .ForMember(dest => dest.CreateData, opt => opt.MapFrom(src => System.DateTime.UtcNow));
            
            CreateMap<int, GetListQuery>();
            
            CreateMap<string, GetListByNameQuery>();
            
            CreateMap<int, DeleteListComand>();

            CreateMap<(AddListDTO, string), UpdateListComand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Item1));

            CreateMap<UpdateListComand, ProductsList>()
                .ForMember(dest => dest.CreateData, opt => opt.MapFrom(src => System.DateTime.UtcNow))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Model.Price))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Model.Weight))
                .ForMember(dest => dest.HowPackeges, opt => opt.MapFrom(src => src.Model.Weight / 8 + 1));

        }

    }
}
