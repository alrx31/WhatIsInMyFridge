using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetListByNameQuery
        (
            string name
        ):IRequest<ProductsList>
    {
        public string Name = name;
    }
}
