namespace DataLabeling.Api.Common.Authentication
{
    public class UserAccessor : IUserAccessor
    {
        public bool IsAuthenticated => User != null;

        public UserTokenData? User { get; set; }
    }
}
