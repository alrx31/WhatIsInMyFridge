using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateProductComand:IRequest
    {
        public string Id { get; set; }

        public AddProductDTO Model { get; set; }
        public UpdateProductComand(string id, AddProductDTO model)
        {
            Id = id;
            Model = model;
        }

        public UpdateProductComand() { }
    }
}
