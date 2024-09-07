using Domain.Entities;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateListComand
        (
            int id,
            string name,
            int weight,
            decimal price,
            List<Product> products
        ):IRequest
    {
        public int Id = id;
        public string Name = name;
        public int Weight = weight;
        public decimal Price = price;
        public List<Product> Products = products;
    }
}
