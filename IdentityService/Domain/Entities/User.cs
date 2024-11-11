using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public bool isAdmin { get; set; }

    }
}
