using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.DTO
{
    public class LogoutModel
    {
        public string? Token { get; set; }
        public int UserId { get; set; }
    }
}
