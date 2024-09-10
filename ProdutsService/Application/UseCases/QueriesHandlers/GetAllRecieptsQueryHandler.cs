using Application.Exceptions;
using Application.MappingProfiles;
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
    public class GetAllRecieptsQueryHandler
        (
            IRecieptsRepository recieptsRepository,
            IMapper mapper
        ):IRequestHandler<GetAllRecieptsQuery,List<Reciept>>
    {

        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<List<Reciept>> Handle(GetAllRecieptsQuery request, CancellationToken cancellationToken)
        {
            if (request.Count < 1)
            {
                throw new ValidationDataException("Count must be greater than 0");
            }

            if(request.Page < 1)
            {
                throw new ValidationDataException("Page must be greater than 0");
            }

            return await _recieptsRepository.GetAllReciepts(request.Page, request.Count);
        }

    }
}
