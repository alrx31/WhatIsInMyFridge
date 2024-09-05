using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class AddProductDTO
    {
        public string Name {get;set;}
        public decimal PricePerKilo {get;set;}
        public TimeSpan ExpirationTime {get;set;}
    }
}
