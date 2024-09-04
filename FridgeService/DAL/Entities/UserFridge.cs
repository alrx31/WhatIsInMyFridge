using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class UserFridge
    {
        public int id { get; set; }
        
        public int userId { get; set; }

        public int fridgeId { get; set; }
        public Fridge fridge { get; set; }

        public DateTime LinkTime { get; set; }
    }
}
