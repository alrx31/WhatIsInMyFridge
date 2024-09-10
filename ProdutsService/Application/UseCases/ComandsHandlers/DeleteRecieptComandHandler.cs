using Application.Exceptions;
using Application.UseCases.Comands;
using AutoMapper;
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
            var reciept = await _repository.GetReciept(request.Id);

            if (reciept is null)
            {
                throw new NotFoundException("Reciept not found");
            }

            await _repository.DeleteReciept(reciept);
        }
    }
}
