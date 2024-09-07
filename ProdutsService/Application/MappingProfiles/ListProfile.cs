using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles
{
    public class ListProfile:Profile
    {
        public ListProfile()
        {
            CreateMap<AddListDTO, AddListComand>();
            CreateMap<AddListComand, ProductsList>();
            CreateMap<int, GetListQuery>();
            CreateMap<string, GetListByNameQuery>();
            CreateMap<int, DeleteListComand>();
            CreateMap<UpdateListComand, ProductsList>();
        }

    }
}
