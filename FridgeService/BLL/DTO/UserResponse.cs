namespace BLL.DTO
{
    public class UserResponse
    {

        public string name { get; set; }
        public string login { get; set; }
        public string email { get; set; }

        //public List<UserFridge> fridgeModelId { get; set; }

        public bool isAdmin { get; set; }

    }
}
