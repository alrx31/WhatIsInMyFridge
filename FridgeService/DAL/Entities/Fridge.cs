using System.Data;

namespace DAL.Entities
{
    public class Fridge
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model { get; set; }

        public string? serial { get; set; }
        
        public DataSetDateTime boughtDate { get; set; }
        public int boxNumber { get; set; } = 0;

        public List<UserFridge> userModelIds { get; set; }

        public List<ProductFridgeModel> products { get; set; }

    }
}
