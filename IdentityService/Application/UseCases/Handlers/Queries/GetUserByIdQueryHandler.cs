using Application.DTO;
using Application.Exceptions;
using Application.Services;
using Application.UseCases.Queries;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Queries
{
    public class GetUserByIdQueryHandler(
        ICacheRepository cacheRepository,
        IUserRepository userRepository
        ): IRequestHandler<GetUserQueryByIdQuery,User>
    {

        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly IUserRepository _repository = userRepository;

        public async Task<User> Handle(GetUserQueryByIdQuery request, CancellationToken cancellationToken)
        {
            var user1 = await _cacheRepository.GetCacheData<User>($"user-{request.Id}");
            if (user1 != null)
            {
                return user1;
            }

            var user = await _repository.getUserById(request.Id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            return user;
        }
    }
}
