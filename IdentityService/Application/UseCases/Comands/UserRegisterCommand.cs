using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class UserRegisterCommand(
        string name,
        string login,
        string email,
        string password
        ):IRequest
    {
        public string Name { get; set; } = name;
        public string Login { get; set; } = login;
        public string Email { get; set; } = email;
        public string Password { get; set; } = password;

    }
}
