using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Domain.Entities
{
    public class ProductInReciept
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RecieptId { get; set; }
        public int Weight { get; set; }
    }
}
