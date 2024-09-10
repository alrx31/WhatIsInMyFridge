using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var reciept = await _recieptsRepository.GetReciept(request.Id);
        
            if(reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            await _recieptsRepository.UpdateReciept(_mapper.Map<Reciept>(request));
        }
    }
}
