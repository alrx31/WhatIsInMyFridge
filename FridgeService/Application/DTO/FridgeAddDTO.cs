using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class FridgeAddDTO
    {

        public string name { get; set; }
        public string model { get; set; }

        public string? serial { get; set; }

        public DataSetDateTime boughtDate { get; set; }
        public int boxNumber { get; set; } = 0;
    }
}
