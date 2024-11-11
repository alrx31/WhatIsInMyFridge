using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetListProductsInListQuery : IRequest<List<ProductInList>>
    {
        public string ListId { get; set; }

        public GetListProductsInListQuery(string listId)
        {
            ListId = listId;
        }

        public GetListProductsInListQuery() { }
    }
}
