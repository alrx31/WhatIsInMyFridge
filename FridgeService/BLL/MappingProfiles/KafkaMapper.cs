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

            CreateMap<(string, int), DAL.Entities.MessageBrokerEntities.ProductRemove>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.FridgeId, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => 0));
            
            CreateMap<(string, int,int), DAL.Entities.MessageBrokerEntities.ProductRemove>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Item1))
                .ForMember(dest => dest.FridgeId, opt => opt.MapFrom(src => src.Item2))
                .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Item3));
        }
    }
}
