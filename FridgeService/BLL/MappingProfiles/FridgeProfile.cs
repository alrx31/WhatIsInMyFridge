using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    public class FridgeProfile:Profile
    {
        public FridgeProfile()
        {
            CreateMap<FridgeAddDTO, Fridge>()
                .ForMember(f => f.boughtDate, opt => opt.MapFrom(src => src.BoughtDate));

            CreateMap<FridgeAddDTO, Fridge>();
        }
    }
}
