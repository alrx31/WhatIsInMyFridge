using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserResponse
    {

        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }

        public List<UserFridge> fridgeModelId { get; set; }

        public bool isAdmin { get; set; }

    }
}
