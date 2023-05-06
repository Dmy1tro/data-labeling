using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DataLabeling.ApiService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((hostContext, confBuilder) =>
                    {
                        var envName = hostContext.HostingEnvironment.EnvironmentName;
                        confBuilder.AddJsonFile("appsettings.json", false, true);
                        confBuilder.AddJsonFile($"appsettings.{envName}.json", true, true);
                    });
                });
    }
}
