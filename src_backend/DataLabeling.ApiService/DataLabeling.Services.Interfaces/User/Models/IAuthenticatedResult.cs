namespace DataLabeling.Services.Interfaces.User.Models
{
    public interface IAuthenticatedResult
    {
        IUserModel User { get; }

        public bool IsSuccess { get; }

        public string FailureReason { get; }
    }
}
