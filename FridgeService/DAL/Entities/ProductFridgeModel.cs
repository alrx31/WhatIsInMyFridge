namespace DAL.Entities
{
    public class ProductFridgeModel
    {
        public int id { get; set; }

        public string productId { get; set; }

        public int count { get; set; }

        public int fridgeId { get; set; }
        public Fridge Fridge { get; set; }

        public DateTime? addTime { get; set; }
    }
}
