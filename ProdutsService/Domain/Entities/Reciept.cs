using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Reciept
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan CookDuration { get; set; }
        public int Portions { get; set; }
        public int Kkal { get; set; }

        //public List<ProductInReciept>? Products { get; set; }
    }
}
