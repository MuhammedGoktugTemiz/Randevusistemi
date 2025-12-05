namespace RandevuWeb.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
    }
}

