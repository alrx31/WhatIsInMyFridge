using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int id { get; set; }

        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public List<UserFridge> fridgeModelId { get; set; }

        public bool isAdmin { get; set; }

    }
}
