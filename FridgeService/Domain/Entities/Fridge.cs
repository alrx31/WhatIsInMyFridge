using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Fridge
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model { get; set; }

        public string? serial { get; set; }
        
        public DataSetDateTime boughtDate { get; set; }
        public int boxNumber { get; set; } = 0;

        public List<UserFridge> userModelIds { get; set; }

        public List<ProductFridgeModel> products { get; set; }

    }
}
