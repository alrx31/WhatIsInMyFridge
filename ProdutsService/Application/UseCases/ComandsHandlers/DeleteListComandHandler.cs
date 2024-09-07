using Application.UseCases.Comands;
using Domain.Repository;

namespace Application.UseCases.ComandsHandlers
{
    public class DeleteListComandHandler
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
