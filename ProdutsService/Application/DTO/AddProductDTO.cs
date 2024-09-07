namespace Application.DTO
{
    public class AddProductDTO
    {
        public string Name {get;set;}
        public decimal PricePerKilo {get;set;}
        public TimeSpan ExpirationTime {get;set;}
    }
}
