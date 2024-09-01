using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class UpdateUserCommand:IRequest<User>
    {
        public UserRegisterCommand user { get; set; } 
        public int id { get; set; }

        public UpdateUserCommand(UserRegisterCommand user, int id)
        {
            this.user = user;
            this.id = id;
        }

        public UpdateUserCommand()
        {
        }
    }
}
