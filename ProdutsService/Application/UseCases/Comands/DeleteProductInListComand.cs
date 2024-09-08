using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
