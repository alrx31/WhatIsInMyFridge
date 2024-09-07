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
            await _listRepository.DeleteListById(request.Id);
        }
    }
}
