using Application.Exceptions;
using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.QueriesHandlers
{
    public class GetRecieptQueryHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<GetRecieptQuery,Reciept>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Reciept> Handle(GetRecieptQuery request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetByIdAsync(request.RecieptId, cancellationToken);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            return reciept;
        }
    }
}
