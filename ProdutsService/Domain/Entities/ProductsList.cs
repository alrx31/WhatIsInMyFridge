using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class ProductsList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public int FridgeId { get; set; }
        public int BoxNumber { get; set; } = 0;
        public string Name { get; set; }
        public int Weight { get; set; }
        public int HowPackeges { get; set; }
        public decimal Price { get; set; }
        public DateTime CreateData { get; set; }
    }
}
