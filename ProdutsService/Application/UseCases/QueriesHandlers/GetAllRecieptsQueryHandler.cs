using Application.Exceptions;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetAllRecieptsQueryHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<GetAllRecieptsQuery,List<Reciept>>
    {

        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<Reciept>> Handle(GetAllRecieptsQuery request, CancellationToken cancellationToken)
        {
            return await _recieptsRepository.GetAllRecieptsPaginationAsync(request.Page, request.Count,cancellationToken);
        }

    }
}
