using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class UpdateListComandHandler
        (
            IListRepository listRepository,
            IMapper mapper
        ):IRequestHandler<UpdateListComand>
    {
        private readonly IListRepository _listRepository = listRepository;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdateListComand request, CancellationToken cancellationToken)
        {
            var list = await _listRepository.GetByIdAsync(request.Id,cancellationToken);

            if(list is null)
            {
                throw new NotFoundException("List not found");
            }
            
            _mapper.Map(request, list);
            
            await _listRepository.UpdateAsync(list, cancellationToken);
        }
    }
}
