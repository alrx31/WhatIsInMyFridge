using Application.Exceptions;
using Application.UseCases.Comands;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteListComandHandler:IRequestHandler<DeleteListComand>
    {
        private readonly IListRepository _listRepository;
        
        public DeleteListComandHandler(IListRepository listRepository)
        {
            _listRepository = listRepository;
        }
        public async Task Handle(DeleteListComand request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetByIdAsync(request.Id,cancellationToken);

            if(list is null)
            {
                throw new NotFoundException("List not found");
            }

            await _listRepository.DeleteAsync(request.Id,cancellationToken);
        }
    }
}
