using MediatR;

namespace Application.UseCases.Comands
{
    public class DeleteProductComand(
        string id
        ):IRequest
    {
        public string Id = id;
    }
}
