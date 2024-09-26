using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetProductsFromRecieptQuery:IRequest<List<ProductInReciept>>
    {
        public string RecieptId { get; }

        public GetProductsFromRecieptQuery(string recieptId)
        {
            RecieptId = recieptId;
        }

        public GetProductsFromRecieptQuery() { }
    }
}
