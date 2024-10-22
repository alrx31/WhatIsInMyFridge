using Domain.Entities;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class SuggestRecieptsQuery : IRequest<Reciept>
    {
        public List<ProductInReciept> products { get; set; }

        public SuggestRecieptsQuery(List<ProductInReciept> products)
        {
            this.products = products;
        }

        public SuggestRecieptsQuery() { }
    }
}
