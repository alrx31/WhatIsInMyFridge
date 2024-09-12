using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetListQuery
        (
            string id
        ):IRequest<ProductsList>
    {
        public string Id = id;
    }
}
