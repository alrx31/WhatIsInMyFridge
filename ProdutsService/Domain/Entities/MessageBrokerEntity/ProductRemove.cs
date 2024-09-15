using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities.MessageBrokerEntities
{
    public class ProductRemove
    {
        public string ProductId { get; set; }
        public int FridgeId { get; set; }
        public int Count { get; set;  }

        public ProductRemove(
            string productId, 
            int fridgeId,
            int count
            )
        {
            ProductId = productId;
            FridgeId = fridgeId;
            Count = count; 
        }

        public ProductRemove() { }
    }
}
