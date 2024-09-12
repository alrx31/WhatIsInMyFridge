using Application.UseCases.Comands;
using Domain.Repository;
using MediatR;

namespace Application.UseCases.Handlers.Comands
{
    public class UserLogoutComandHandler(
        
        IUnitOfWork unitOfWork
        
        ):IRequestHandler<UserLogoutCommand>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserRepository.CanselRefreshToken(request.UserId);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
