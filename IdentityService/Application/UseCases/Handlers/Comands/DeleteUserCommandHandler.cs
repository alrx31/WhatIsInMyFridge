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
    public class DeleteUserCommandHandler(
            IUserService userService
        ):IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserService _userService = userService;

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userService.DeleteUser(request.id,request.initiatorId);

            return Unit.Value;
        }

    }
}
