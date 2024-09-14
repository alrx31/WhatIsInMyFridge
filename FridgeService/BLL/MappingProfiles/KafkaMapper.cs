using AutoMapper;
using BLL.DTO;
using DAL.Entities;
using DAL.Entities.MessageBrokerEntities;


namespace BLL.MappingProfiles
{
    public class KafkaMapper:Profile
    {
        public KafkaMapper()
        {
            CreateMap<ProductFridgeModel, ProductInfo>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.productId))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.count));

            CreateMap<(string productId, int fridgeId), DAL.Entities.MessageBrokerEntities.Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.productId))
                .ForMember(dest => dest.FridgeId, opt => opt.MapFrom(src => src.fridgeId));
        }
    }
}
