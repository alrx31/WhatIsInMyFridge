namespace DAL.Entities.MessageBrokerEntities
{
    public class Product
    {
        public List<ProductInfo> ProductId { get; set; }
        public int FridgeId { get; set; }

        public Product(List<ProductInfo> productId, int fridgeId)
        {
            ProductId = productId;
            FridgeId = fridgeId;
        }
    }
}
