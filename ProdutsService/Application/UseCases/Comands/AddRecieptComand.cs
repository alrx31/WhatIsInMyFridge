using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class AddRecieptComand:IRequest
    {
        public AddRecieptDTO Model { get; set; }

        public AddRecieptComand(AddRecieptDTO model)
        {
            Model = model;
        }

        public AddRecieptComand() { }
    }
}
