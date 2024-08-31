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
    public class UserLoginComandHandler: IRequestHandler<UserLoginCommand, LoginResponse>
    {
        private readonly IAuthService _authService;

        public UserLoginComandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginResponse> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginUser(request.login,request.password);
            
            return token;
        }
    }
}
