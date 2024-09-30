using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.QueriesHandlers
{
    public class GetAllListsQueryHandler
        (
            IListRepository listRepository
        ) : IRequestHandler<GetAllListsQuery, List<ProductsList>>
    {
        public async Task<List<ProductsList>> Handle(GetAllListsQuery request, CancellationToken cancellationToken)
        {
            return (List<ProductsList>)await listRepository.GetAllAsync(cancellationToken);
        }
    }
}
