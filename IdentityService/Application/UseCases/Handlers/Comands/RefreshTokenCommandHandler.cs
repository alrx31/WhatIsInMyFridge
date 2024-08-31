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
    public class RefreshTokenCommandHandler:IRequestHandler<RefreshTokenCommand,LoginResponse>
    {
        private readonly IAuthService _authService;

        public RefreshTokenCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshToken(request.JwtToken);
        }
    }
}
