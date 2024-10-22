using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetProductQuery(
        string id
        ):IRequest<Product>
    {
        public string Id = id;
    }
}
