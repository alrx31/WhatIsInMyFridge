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
    public class UserLogoutComandHandler(
        
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
        
        ):IRequestHandler<UserLogoutCommand>
    {

        private readonly IUserRepository _repository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Unit> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            await _repository.CanselRefreshToken(request.UserId);

            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
