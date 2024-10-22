using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class AddListComand
        (
            AddListDTO model
        ) : IRequest
    {
        public AddListDTO Model = model;
    }
}
