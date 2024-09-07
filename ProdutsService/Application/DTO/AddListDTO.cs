using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AddListDTO
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public decimal Price { get; set; }

        public List<Product>? Products { get; set; }
    }
}
