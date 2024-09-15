using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllProductsQuery:IRequest<List<Product>>
    {
        public int Page { get; set; }
        public int Count { get; set; }

        public GetAllProductsQuery(int page, int count)
        {
            Page = page;
            Count = count;
        }

        public GetAllProductsQuery()
        {
        }
    }
}
