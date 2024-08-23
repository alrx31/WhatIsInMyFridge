using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; } = false;
        public User User { get; set; }
        public string JwtToken { get; set; }
        //public string RefreshToken { get; set; }
    }
}
