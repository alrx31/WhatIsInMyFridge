using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class RecieptsProfile:Profile
    {
        public RecieptsProfile()
        {
            CreateMap<AddRecieptDTO, AddRecieptComand>();
            CreateMap<AddRecieptComand, Reciept>();
            CreateMap<string, GetRecieptQuery>();
            CreateMap<string, DeleteRecieptComand>();
            CreateMap<(AddRecieptDTO,string),UpdateRecieptComand>()
                .ConstructUsing(x=>new UpdateRecieptComand(x.Item2,x.Item1.Name,x.Item1.CookDuration,x.Item1.Portions,x.Item1.Kkal));
            CreateMap<UpdateRecieptComand,Reciept>();
        }
    }
}
