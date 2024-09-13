using Application.Exceptions;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetListQueryHandler:IRequestHandler<GetListQuery, ProductsList>
    {
        private readonly IListRepository _listRepository;

        public GetListQueryHandler(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }

        public async Task<ProductsList> Handle(GetListQuery request, CancellationToken cancellationToken)
        {
            var res = await _listRepository.GetByIdAsync(request.Id,cancellationToken);
            
            if(res is null)
            {
                throw new NotFoundException("List not found");
            }

            return res;
        }
    }
}
