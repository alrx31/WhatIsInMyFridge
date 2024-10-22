﻿
namespace DAL.Entities
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerKilo { get; set; }
 
        public int Count { get; set; }

        public TimeSpan ExpirationTime { get; set; }
        public DateTime? AddTime { get; set; }


    }
}
