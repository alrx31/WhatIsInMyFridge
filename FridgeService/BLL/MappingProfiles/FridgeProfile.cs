using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    public class FridgeProfile:Profile
    {
        public FridgeProfile()
        {
            CreateMap<FridgeAddDTO, Fridge>();
            CreateMap<(FridgeAddDTO, int), Fridge>()
                .ForMember(f => f.id, opt => opt.MapFrom(src => src.Item2))
                .ForMember(f => f.serial, opt => opt.MapFrom(src => src.Item1.Serial))
                .ForMember(f => f.boxNumber, opt => opt.MapFrom(src => src.Item1.BoxNumber))
                .ForMember(f => f.boughtDate, opt => opt.MapFrom(src => src.Item1.BoughtDate))
                .ForMember(f => f.model, opt => opt.MapFrom(src => src.Item1.Model))
                .ForMember(f => f.name, opt => opt.MapFrom(src => src.Item1.Name));
        }
    }
}
