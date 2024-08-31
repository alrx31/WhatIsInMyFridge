using Application.Services;
using Application.UseCases.Comands;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Comands
{
    public class UpdateUserCommandHandler(
            IUserService userService
        ):IRequestHandler<UpdateUserCommand,User>
    {
        private readonly IUserService _userService = userService;

        public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            //TODO: 
            return await _userService.UpdateUser(request.user, request.id);
        }

    }
}
