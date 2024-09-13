using Application.DTO;
using Domain.Entities;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateListComand:IRequest
    {
        public string Id { get; set; }
        public AddListDTO Model { get; set; }
        public List<Product> Products { get; set; }
    
        public UpdateListComand(string id, AddListDTO model, List<Product> products)
        {
            Id = id;
            Model = model;
            Products = products;
        }

        public UpdateListComand() { }
    }
}
