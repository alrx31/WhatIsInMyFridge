using Application.UseCases.Comands;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.ComandsHandlers
{
    public class AddListComandHandler
        (
            IListRepository listRepository,
            IMapper mapper
        ): IRequestHandler<AddListComand>
    {
        private readonly IListRepository _listRepository = listRepository; 
        private readonly IMapper _mapper = mapper;

        public async Task Handle(AddListComand request, CancellationToken cancellationToken)
        {
            await _listRepository.AddList(_mapper.Map<ProductsList>(request));
        }
    }
}
