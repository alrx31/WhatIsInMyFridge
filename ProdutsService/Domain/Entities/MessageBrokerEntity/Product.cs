using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.MessageBrokerEntity
{
    public class Product
    {
        public List<string> ProductId { get; set; }
        public int FridgeId { get; set; }

        public Product(List<string> productId, int fridgeId)
        {
            ProductId = productId;
            FridgeId = fridgeId;
        }
    }
}
