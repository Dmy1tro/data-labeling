using DataLabeling.Services.Interfaces.User.Models;

namespace DataLabeling.Services.User.Models
{
    public class AuthenticatedResult : IAuthenticatedResult
    {
        public IUserModel User { get; set; }

        public bool IsSuccess { get; set; }

        public string FailureReason { get; set; }
    }
}
