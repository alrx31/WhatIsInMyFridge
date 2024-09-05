using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductComand, Product>();
            CreateMap<AddProductDTO, AddProductComand>();
            CreateMap<AddProductDTO, UpdateProductComand>();
            CreateMap<int, GetProductQuery>();
            CreateMap<int,DeleteProductComand>();
            CreateMap<UpdateProductComand,Product>();
            CreateMap<(int,int), GetAllProductsQuery>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Item2));
        }
    }
}
