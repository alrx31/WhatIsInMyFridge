using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.QueriesHandlers
{
    public class SuggestRecieptsQueryHandler
        (
            IMapper mapper,
            IRecieptsRepository recieptsRepository,
            ILogger<SuggestRecieptsQueryHandler> logger
        ) :IRequestHandler<SuggestRecieptsQuery,Reciept>
    {
        private readonly IRecieptsRepository _recieptsRepository = recieptsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<SuggestRecieptsQueryHandler> _logger = logger;

        public async Task<Reciept> Handle(SuggestRecieptsQuery request, CancellationToken cancellationToken)
        {
            if (request.products.Count == 0)
            {
                throw new BadRequestException("Products list is empty");
            }

            var reciepts = (await _recieptsRepository.GetAllAsync(cancellationToken)).ToList();
            
            if (reciepts.Count == 0)
            {
                throw new NotFoundException("No reciepts available.");
            }

            var validReciepts = reciepts.Where(reciept =>
                reciept.Products.All(rp =>
                    request.products.Any(p =>
                        p.Id == rp.Id && p.Weight >= rp.Weight)))
                .ToList();

            if (!validReciepts.Any())
            {
                throw new NotFoundException("No suitable reciepts found.");
            }

            var random = new Random();

            var randomIndex = random.Next(0, validReciepts.Count);

            return validReciepts[randomIndex];
        }
    }
}
