using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetRecieptQuery:IRequest<Reciept>
    {
        public string RecieptId { get; set; }
    
        public GetRecieptQuery(string recieptId)
        {
            RecieptId = recieptId;
        }

        public GetRecieptQuery()
        {
        }
    }
}
