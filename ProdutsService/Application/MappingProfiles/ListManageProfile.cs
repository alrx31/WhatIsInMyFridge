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
            CreateMap<(AddProductToListDTO,string), AddProductToListComand>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1.ProductId))
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Item1.Weight))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Item1.Cost));
            
            CreateMap<string, GetListProductsQuery>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src));

            CreateMap<(string,string), DeleteProductInListComand>()
                .ForMember(dest => dest.ListId, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item2));
            
            CreateMap<AddProductToListComand, ProductInList>();
        }
    }
}
