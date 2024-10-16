using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class RefreshTokenModel
    {
        [Key]
        public int id { get; set; }

        public int userId{ get; set; }
        public User? user{ get; set; }

        public string email { get; set; }

        public string? refreshToken { get; set; }
        public DateTime refreshTokenExpiryTime { get; set; }
    }
}
