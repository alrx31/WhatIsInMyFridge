using Application.DTO;
using Application.UseCases.Comands;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using System.Net.Http.Headers;

namespace Application.MappingProfiles
{
    public class RecieptsProfile:Profile
    {
        public RecieptsProfile()
        {
            CreateMap<AddRecieptDTO, AddRecieptComand>()
                .ConstructUsing(x => new AddRecieptComand(x));

            CreateMap<AddRecieptComand, Reciept>();

            CreateMap<AddRecieptDTO, Reciept>();

            CreateMap<string, GetRecieptQuery>()
                .ForMember(dest => dest.RecieptId, opt => opt.MapFrom(src => src));
            
            CreateMap<string, DeleteRecieptComand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src));
            
            CreateMap<(AddRecieptDTO,string),UpdateRecieptComand>()
                .ConstructUsing(x=>new UpdateRecieptComand(x.Item2,x.Item1));

            CreateMap<UpdateRecieptComand, Reciept>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Model.Name))
                .ForMember(dest => dest.Kkal, opt => opt.MapFrom(src => src.Model.Kkal))
                .ForMember(dest => dest.CookDuration, opt => opt.MapFrom(src => src.Model.CookDuration))
                .ForMember(dest => dest.Portions, opt => opt.MapFrom(src => src.Model.Portions));

            CreateMap<(int, int), GetAllRecieptsQuery>()
                .ConstructUsing(x => new GetAllRecieptsQuery(x.Item1, x.Item2));
        
            CreateMap<AddProductToRecieptDTO,AddProductToRecieptComand>()
                .ConstructUsing(x => new AddProductToRecieptComand(x));

            CreateMap<DeleteProductFromRecieptDTO, DeleteProductFromRecieptComand>()
                .ConstructUsing(x => new DeleteProductFromRecieptComand(x.RecieptId,x.ProductId));

            CreateMap<string, GetProductsFromRecieptQuery>()
                .ConstructUsing(x => new GetProductsFromRecieptQuery(x));
        }
    }
}
