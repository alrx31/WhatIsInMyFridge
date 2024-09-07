using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetListByNameQuery:IRequest<ProductsList>
    {
        public string Name { get; set; }

        public GetListByNameQuery(string name)
        {
            Name = name;
        }

        public GetListByNameQuery() { }
    }
}
