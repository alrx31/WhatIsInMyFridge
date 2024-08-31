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
    internal class UserRegisterComandHandler(
        IAuthService authService
        ):IRequestHandler<UserRegisterCommand>
    {
        private readonly IAuthService _authService = authService;
        
        public async Task<Unit> Handle(UserRegisterCommand request,CancellationToken cancellationToken)
        {
            await _authService.RegisterUser(request);
            return Unit.Value;
        }

    }
}
