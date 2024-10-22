using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class UpdateRecieptComandHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<UpdateRecieptComand>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateRecieptComand request, CancellationToken cancellationToken)
        {
            var reciept = await _recieptsRepository.GetByIdAsync(request.Id,cancellationToken);
        
            if(reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            await _recieptsRepository.UpdateAsync(_mapper.Map<Reciept>(request),cancellationToken);
        }
    }
}
