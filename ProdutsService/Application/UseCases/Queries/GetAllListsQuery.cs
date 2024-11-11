using Domain.Entities;
using MediatR;

namespace Application.UseCases.Queries
{
    public class GetAllListsQuery:IRequest<List<ProductsList>>
    {
    }
}
