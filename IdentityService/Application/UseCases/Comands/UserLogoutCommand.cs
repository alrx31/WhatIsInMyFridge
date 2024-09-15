using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class UserLogoutCommand:IRequest
    {
        public int UserId { get; set; }

        public UserLogoutCommand(int userId)
        {
            UserId = userId;
        }
        public UserLogoutCommand()
        {
        }
    }
}
