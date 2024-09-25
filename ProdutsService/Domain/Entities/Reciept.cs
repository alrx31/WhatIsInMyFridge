using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class Reciept
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public TimeSpan CookDuration { get; set; }
        public int Portions { get; set; }
        public int Kkal { get; set; }

        public List<Product> Products { get; set; }
    }
}
