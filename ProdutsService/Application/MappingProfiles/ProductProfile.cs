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
            CreateMap<(AddProductDTO,string), UpdateProductComand>()
                .ForMember(dest => dest.Id, opt=>opt.MapFrom(src=>src.Item2))
                .ForMember(dest => dest.Name, opt=>opt.MapFrom(src=>src.Item1.Name))
                .ForMember(dest => dest.PricePerKilo, opt=>opt.MapFrom(src=>src.Item1.PricePerKilo))
                .ForMember(dest => dest.ExpirationTime, opt=>opt.MapFrom(src=>src.Item1.ExpirationTime));

            CreateMap<int, GetProductQuery>();
            CreateMap<int,DeleteProductComand>();
            CreateMap<UpdateProductComand, Product>();
            CreateMap<(int,int), GetAllProductsQuery>()
                .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Item2));
        }
    }
}
