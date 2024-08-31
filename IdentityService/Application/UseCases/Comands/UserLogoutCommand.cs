using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class UserLogoutCommand(
        int userId
        ):IRequest
    {
        public int UserId { get; set; } = userId;
    }
}
