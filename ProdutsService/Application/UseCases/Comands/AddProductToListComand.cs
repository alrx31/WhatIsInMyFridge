using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class AddProductToListComand:IRequest
    {
        public string ListId { get; set; }
        public AddProductToListDTO Model { get; set; }

        public AddProductToListComand(string listId, AddProductToListDTO model)
        {
            listId = ListId;
            Model = model;
        }

        public AddProductToListComand() { }
    }
}
