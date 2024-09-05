using Application.DTO;
using Application.UseCases.Comands;
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
        }
    }
}
