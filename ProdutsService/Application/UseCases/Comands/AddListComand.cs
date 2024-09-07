using Domain.Entities;
using MediatR;

namespace Application.UseCases.Comands
{
    public class AddListComand
        (
            string name,
            int weight,
            decimal price
        ): IRequest
    {
        public string Name = name;
        public int Weight = weight;
        public decimal Price = price;
    }
}
