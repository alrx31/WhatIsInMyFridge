using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductFridgeModel
    {
        public int id { get; set; }
        

        // TODO: may be Guid
        public int productId { get; set; }

        public int count { get; set; }

        public int fridgeId { get; set; }
        public Fridge Fridge { get; set; }

    }
}
