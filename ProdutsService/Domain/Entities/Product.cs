﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerKilo { get; set; }
        public TimeSpan ExpirationTime { get; set; }

        public List<ProductInList>? Lists { get; set; }
    }

    public class ProductInReciept: Product
    {
        public int Weight { get; set; }

        public ProductInReciept(Product product, int weight)
        {
            Id = product.Id;
            Name = product.Name;
            PricePerKilo = product.PricePerKilo;
            ExpirationTime = product.ExpirationTime;
            Lists = product.Lists;
            Weight = weight;
        }
    }
}
