using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class UpdateUserCommand(
        UserRegisterCommand User,
        int Id
        ):IRequest<User>
    {
        public UserRegisterCommand user { get; set; } = User;
        public int id { get; set; } = Id;
    }
}
