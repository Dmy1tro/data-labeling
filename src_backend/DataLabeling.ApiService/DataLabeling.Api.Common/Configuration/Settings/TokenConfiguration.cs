namespace DataLabeling.Api.Common.Configuration.Settings
{
    public class TokenConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }
}
