using Application.Exceptions;
using Application.Services;
using Application.UseCases.Comands;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Comands
{
    public class DeleteUserCommandHandler(
            ICacheRepository cacheRepository,
            IUserRepository repository,
            IUnitOfWork unitOfWork
        ):IRequestHandler<DeleteUserCommand>
    {

        private readonly ICacheRepository _cacheRepository = cacheRepository;
        private readonly IUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _cacheRepository.GetCacheData<User>($"user-{request.InitiatorId}");

            if (user is null)
            {
                user = await _repository.getUserById(request.Id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var token = await _repository.getTokenModel(user.email);

            if (user.isAdmin || user.id == request.Id)
            {
                await _cacheRepository.RemoveCacheData($"user-{request.Id}");
                await _repository.DeleteUser(request.Id);

                if (token != null)
                {
                    await _repository.DeleteRefreshTokenByUserId(request.Id);
                }

                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new BadRequestException("You do not have acess");
            }
            return Unit.Value;
        }

    }
}
