﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductInList
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductsListId { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
    }
}