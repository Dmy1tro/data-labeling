using Microsoft.Extensions.Configuration;

namespace DataLabeling.Api.Common.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetConfiguration<T>(this IConfiguration configuration,
                                            string configName = null)
        {
            configName ??= typeof(T).Name;
            return configuration.GetSection(configName).Get<T>();
        }
    }
}
