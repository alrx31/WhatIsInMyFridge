
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateProductComand:IRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerKilo { get; set; }
        public TimeSpan ExpirationTime { get; set; }
        
        public UpdateProductComand(string id, string name, decimal pricePerKilo, TimeSpan expirationTime)
        {
            Id = id;
            Name = name;
            PricePerKilo = pricePerKilo;
            ExpirationTime = expirationTime;
        }

        public UpdateProductComand() { }
    }
}
