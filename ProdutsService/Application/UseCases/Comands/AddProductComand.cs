using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class AddProductComand
        (
            AddProductDTO model
        ):IRequest
    {
        public AddProductDTO Model { get; set; } = model;
    }
}
