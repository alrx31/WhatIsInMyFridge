using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class LogoutDTO
    {
        public string? Token { get; set; }
        public int UserId { get; set; }
    }
}
