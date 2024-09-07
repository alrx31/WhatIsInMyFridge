using Application.Exceptions;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetListByNameQueryHandler
        (
            IListRepository listRepository
        ):IRequestHandler<GetListByNameQuery, ProductsList>
    {
        private readonly IListRepository _listRepository = listRepository;

        public async Task<ProductsList> Handle(GetListByNameQuery request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetListByName(request.Name);

            if(list == null)
            {
                throw new NotFoundException("List not found");
            }

            return list;
        }

    }
}
