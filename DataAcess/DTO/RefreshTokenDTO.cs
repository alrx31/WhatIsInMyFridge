using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.DTO
{
    public class RefreshTokenDTO
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
