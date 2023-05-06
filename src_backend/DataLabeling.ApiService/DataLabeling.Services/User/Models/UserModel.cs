using DataLabeling.Common.User;
using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class UserModel : IUserModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PinConfirmation { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string Roles { get; set; }

        public UserType UserType { get; set; }
    }
}
