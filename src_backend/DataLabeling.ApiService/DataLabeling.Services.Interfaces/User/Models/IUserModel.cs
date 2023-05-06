using DataLabeling.Common.User;

namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface IUserModel
    {
        public int Id { get; }

        public string Email { get; }

        public bool EmailConfirmed { get; }

        public string PinConfirmation { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string FullName { get; }

        public string Roles { get; }

        public UserType UserType { get; }
    }
}
