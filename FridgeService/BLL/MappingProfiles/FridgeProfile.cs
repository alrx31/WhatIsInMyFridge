using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.MappingProfiles
{
    public class FridgeProfile:Profile
    {
        public FridgeProfile()
        {
            CreateMap<Fridge, FridgeAddDTO>().ReverseMap();
        }
    }
}
