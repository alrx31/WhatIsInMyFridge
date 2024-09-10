using Application.UseCases.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.QueriesHandlers
{
    public class GetRecieptQueryHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<GetRecieptQuery,Reciept>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Reciept> Handle(GetRecieptQuery request, CancellationToken cancellationToken)
        {
            return await _recieptsRepository.GetReciept(request.RecieptId);
        }
    }
}
