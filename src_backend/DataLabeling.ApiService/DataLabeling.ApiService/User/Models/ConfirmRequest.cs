using DataLabeling.Common.User;

namespace DataLabeling.ApiService.User.Models
{
    public class ConfirmRequest
    {
        public string Email { get; set; }

        public string Pin { get; set; }

        public UserType UserType { get; set; }
    }
}
