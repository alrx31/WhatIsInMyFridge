using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Comands
{
    public class RefreshTokenCommand(
        string jwtToken
        ):IRequest<LoginResponse>
    {
        public string JwtToken { get; } = jwtToken;
    }
}
