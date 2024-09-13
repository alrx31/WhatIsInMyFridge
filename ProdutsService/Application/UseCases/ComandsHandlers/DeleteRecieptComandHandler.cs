using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteRecieptComandHandler
        (
            IRecieptsRepository repository,
            IMapper mapper
        ):IRequestHandler<DeleteRecieptComand>
    {
        private readonly IRecieptsRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(DeleteRecieptComand request, CancellationToken cancellationToken)
        {
            var reciept = await _repository.GetByIdAsync(request.Id,cancellationToken);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            await _repository.DeleteAsync(reciept.Id,cancellationToken);
        }
    }
}
