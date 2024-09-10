using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class ProductFridgeModel
    {
        public int id { get; set; }

        public string productId { get; set; }

        public int count { get; set; }

        public int fridgeId { get; set; }
        public Fridge Fridge { get; set; }

        public DateTime? addTime { get; set; }
    }
}
