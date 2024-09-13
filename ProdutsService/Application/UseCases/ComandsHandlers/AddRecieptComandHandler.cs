using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class AddRecieptComandHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<AddRecieptComand>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(AddRecieptComand request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetRecieptByNameAsync(request.Name, cancellationToken);

            if(reciept is not null)
            {
                throw new AlreadyExistsException("Reciept already exists");
            }

            await _recieptsRepository.AddAsync(_mapper.Map<Reciept>(request),cancellationToken);
        }

    }
}
