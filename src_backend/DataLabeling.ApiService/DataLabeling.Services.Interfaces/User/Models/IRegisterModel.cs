using DataLabeling.Common.User;

namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface IRegisterModel
    {
        public string Email { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string Password { get; }

        public UserType UserType { get; }

        public string Roles { get; }
    }
}
