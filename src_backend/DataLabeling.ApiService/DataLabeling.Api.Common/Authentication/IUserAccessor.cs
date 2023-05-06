namespace DataLabeling.Api.Common.Authentication
{
    public interface IUserAccessor
    {
        bool IsAuthenticated { get; }

        public UserTokenData? User { get; set; }
    }
}
