using Application.Exceptions;
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
            var list = await _listRepository.GetListByName(request.Model.Name,cancellationToken);

            if (list is not null) 
            {
                throw new AlreadyExistsException("List already exists");
            }

            await _listRepository.AddAsync(_mapper.Map<ProductsList>(request.Model),cancellationToken);
        }
    }
}
