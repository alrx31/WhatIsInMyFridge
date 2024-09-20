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
            CreateMap<AddProductDTO, Product>();
            
            CreateMap<AddProductDTO, AddProductComand>()
                .ConstructUsing(src => new AddProductComand(src));

            CreateMap<(AddProductDTO,string), UpdateProductComand>()
                .ForMember(dest => dest.Id, opt=>opt.MapFrom(src=>src.Item2))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Item1));

            CreateMap<int, GetProductQuery>();
            
            CreateMap<int,DeleteProductComand>();
            
            CreateMap<UpdateProductComand, Product>();
            
            CreateMap<(int,int), GetAllProductsQuery>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Item2));

            CreateMap<UpdateProductComand,Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ExpirationTime, opt => opt.MapFrom(src => src.Model.ExpirationTime));

        }
    }
}
