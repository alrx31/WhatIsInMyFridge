using Application.DTO;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class FridgeProfile:Profile
    {
        public FridgeProfile()
        {
            CreateMap<Fridge, FridgeAddDTO>().ReverseMap();
        }
    }
}
