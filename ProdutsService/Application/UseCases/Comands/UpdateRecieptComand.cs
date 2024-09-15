using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateRecieptComand:IRequest
    {
        public string Id { get; set; }

        public AddRecieptDTO Model { get; set; }
        
        public UpdateRecieptComand(string id, AddRecieptDTO model)
        {
            Id = id;
            Model = model;
        }
        public UpdateRecieptComand() { }
    }
}
