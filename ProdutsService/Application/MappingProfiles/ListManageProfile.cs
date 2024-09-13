using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ListManageProfile:Profile
    {
        public ListManageProfile()
        {
            CreateMap<(AddProductToListDTO, string), AddProductToListComand>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Item1));


            CreateMap<string, GetListProductsQuery>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src));

            CreateMap<(string,string), DeleteProductInListComand>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item2));

            CreateMap<AddProductToListComand, ProductInList>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.ListId))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Model.ProductId))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Model.Cost))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Model.Weight));
                
        }
    }
}
