
namespace Tests.Integrationtests.FridgeController
{
    internal class FridgeUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public DateTime BoughtDate { get; set; }
        public int BoxNumber { get; set; }
    }
}