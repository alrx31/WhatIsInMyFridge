using MediatR;

namespace Application.UseCases.Comands
{
    public class DeleteListComand
        (
        string id
        ):IRequest
    {
        public string Id = id;
    }
}
