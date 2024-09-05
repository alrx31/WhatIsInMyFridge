using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductInReciept
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RecieptId { get; set; }
        public int Weight { get; set; }
    }
}
