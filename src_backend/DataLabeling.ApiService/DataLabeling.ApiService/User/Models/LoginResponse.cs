namespace DataLabeling.ApiService.User.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int UserType { get; set; }

        public string Roles { get; set; }
    }
}
