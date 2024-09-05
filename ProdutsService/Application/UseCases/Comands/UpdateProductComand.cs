
using MediatR;

namespace Application.UseCases.Comands
{
    public class UpdateProductComand
        (
            string name,
            decimal pricePerKilo,
            TimeSpan expirationTime
        ):IRequest
    {
        public string Name = name;
        public decimal PricePerKilo = pricePerKilo;
        public TimeSpan ExpirationTime = expirationTime;
    }
}
