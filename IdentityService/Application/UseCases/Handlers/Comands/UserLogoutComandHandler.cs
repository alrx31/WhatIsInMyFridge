using Application.Services;
using Application.UseCases.Comands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Comands
{
    public class UserLogoutComandHandler:IRequestHandler<UserLogoutCommand>
    {
        private readonly IAuthService _authService;
        
        public UserLogoutComandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<Unit> Handle(UserLogoutCommand request, CancellationToken cancellationToken)
        {
            await _authService.Logout(request.UserId);
            return Unit.Value;
        }
    }
}
