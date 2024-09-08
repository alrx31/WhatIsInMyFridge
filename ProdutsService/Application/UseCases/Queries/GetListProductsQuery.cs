using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetListProductsQuery:IRequest<List<Product>>
    {
        public string ListId { get; set; }
        
        public GetListProductsQuery(string listId)
        {
            ListId = listId;
        }

        public GetListProductsQuery() { }
    }
}
