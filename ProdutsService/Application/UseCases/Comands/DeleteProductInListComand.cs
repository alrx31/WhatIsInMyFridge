using MediatR;

namespace Application.UseCases.Comands
{
    public class DeleteProductInListComand:IRequest
    {
        public string ListId { get; set; }
        public string ProductId { get; set; }

        public DeleteProductInListComand(string listId, string productId)
        {
            ListId = listId;
            ProductId = productId;
        }

        public DeleteProductInListComand() { }
    }
}
