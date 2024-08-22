using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcess.DTO
{
    public class RegisterDTO
    {
        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
}
