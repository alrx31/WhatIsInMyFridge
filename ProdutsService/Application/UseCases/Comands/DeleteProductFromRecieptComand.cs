using MediatR;

namespace Application.UseCases.Comands
{
    public class DeleteProductFromRecieptComand:IRequest
    {
        public string RecieptId { get; }
        public string ProductId { get; }

        public DeleteProductFromRecieptComand(string recieptId, string productId)
        {
            RecieptId = recieptId;
            ProductId = productId;
        }

        public DeleteProductFromRecieptComand() { }
    }
}
