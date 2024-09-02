using System.Data;

namespace BLL.DTO
{
    public class FridgeAddDTO
    {

        public string name { get; set; }
        public string model { get; set; }

        public string? serial { get; set; }

        public DataSetDateTime boughtDate { get; set; }
        public int boxNumber { get; set; } = 0;
    }
}
