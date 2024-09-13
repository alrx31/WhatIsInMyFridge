﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; }
    }
}
