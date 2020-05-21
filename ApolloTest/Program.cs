using ApolloTest.Conf;
using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.IO;

namespace ApolloTest
{
    public class Program
    {
        private static IHost host;
        public static void Main(string[] args)
        {
            host = CreateHostBuilder(args).Build();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();


            builder = new ConfigurationBuilder().AddApollo(builder.Build().GetSection("apollo")).AddNamespace("spzl.ums");
            var d = builder.Build();


            var appSetting = new ServiceCollection()
               .Configure<AppSettings>(builder.Build());

            var appConf = appSetting.BuildServiceProvider().GetService<IOptions<AppSettings>>();


            return Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, builder) =>
            {
                builder
                .AddApollo(builder.Build().GetSection("apollo")).AddDefault();
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseKestrel();
                    //webBuilder.UseUrls($"http://{appConf.Value.DeployHostName}:{appConf.Value.DeployPort}");
                    webBuilder.UseUrls("http://127.0.0.1:5000");
                });
        }
    }
}
