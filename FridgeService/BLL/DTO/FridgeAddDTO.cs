using System.Data;

namespace BLL.DTO
{
    public class FridgeAddDTO
    {
        public string Name { get; set; }
        public string Model { get; set; }

        public string? Serial { get; set; }

        public DataSetDateTime BoughtDate { get; set; }
        public int BoxNumber { get; set; } = 0;
    }
}
