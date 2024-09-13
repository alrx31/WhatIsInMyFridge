using Application.DTO;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UserLoginCommand(string login, string password) : IRequest<LoginResponse>
    {
        public string Login { get; } = login;
        public string Password { get; } = password;
    }

    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; } = false;
        public UserDTO User { get; set; }
        public string JwtToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}


