using MediatR;

namespace Application.UseCases.Comands
{
    public class AddProductToListComand:IRequest
    {
        public string ListId { get; set; }
        public string ProductId { get; set; }
        public int Weight { get; set; } 
        public decimal Cost { get; set; }

        public AddProductToListComand(string listId,string productId, int weight, decimal cost)
        {
            listId = ListId;
            ProductId = productId;
            Weight = weight;
            Cost = cost;
        }

        public AddProductToListComand() { }
    }
}
