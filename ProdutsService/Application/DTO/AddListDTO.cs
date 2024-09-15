namespace Application.DTO
{
    public class AddListDTO
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public int FridgeId { get; set; }
        public int BoxNumber { get; set; } = 0;

        public decimal Price { get; set; }
    }
}
