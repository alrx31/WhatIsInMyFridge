using Domain.Entities;
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateListComand:IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public decimal Price { get; set; }
        public List<Product> Products { get; set; }
    
        public UpdateListComand(string id, string name, int weight, decimal price, List<Product> products)
        {
            Id = id;
            Name = name;
            Weight = weight;
            Price = price;
            Products = products;
        }

        public UpdateListComand() { }
    }
}
