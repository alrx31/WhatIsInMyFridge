using Application.Exceptions;
using Application.UseCases.Comands;
using Domain.Entities;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.Handlers.Comands
{
    public class DeleteUserCommandHandler(
            IUnitOfWork unitOfWork
        ):IRequestHandler<DeleteUserCommand>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.GetCacheData<User>($"user-{request.InitiatorId}");

            if (user is null)
            {
                user = await _unitOfWork.GetUserById(request.Id);
            }

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var token = await _unitOfWork.GetTokenModel(user.email);

            if (user.isAdmin || user.id == request.Id)
            {
                await _unitOfWork.RemoveCacheData($"user-{request.Id}");

                await _unitOfWork.DeleteUser(request.Id);

                if (token != null)
                {
                    await _unitOfWork.DeleteRefreshTokenByUserId(request.Id);
                }
            }
            else
            {
                throw new BadRequestException("You do not have acess");
            }

            return Unit.Value;
        }

    }
}
