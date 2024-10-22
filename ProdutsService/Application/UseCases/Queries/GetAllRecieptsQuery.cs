using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllRecieptsQuery:IRequest<List<Reciept>>
    {
        public int Page { get; set; }
        public int Count { get; set; }

        public GetAllRecieptsQuery(int page, int count)
        {
            Page = page;
            Count = count;
        }
        public GetAllRecieptsQuery() { }
    }
}
