using DataLabeling.Common.User;
using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class RegisterModel : IRegisterModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }

        public UserType UserType { get; set; }

        public string Roles { get; set; }
    }
}
