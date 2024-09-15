using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshTokenModel
    {
        public int id { get; set; }

        public int userId{ get; set; }
        public User? user{ get; set; }

        public string email { get; set; }

        public string? refreshToken { get; set; }
        public DateTime refreshTokenExpiryTime { get; set; }
    }
}
