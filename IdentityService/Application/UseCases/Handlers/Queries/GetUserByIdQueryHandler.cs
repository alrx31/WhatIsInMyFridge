using Application.DTO;
using Application.Services;
using Application.UseCases.Queries;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Handlers.Queries
{
    public class GetUserByIdQueryHandler(
        IUserService userService
        ): IRequestHandler<GetUserQueryByIdQuery,User>
    {
        private readonly IUserService _userService = userService;

        public async Task<User> Handle(GetUserQueryByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.getUserById(request.id);

            return user;
        }
    }
}
