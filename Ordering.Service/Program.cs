using Com.Ctrip.Framework.Apollo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Domain.Conf;
using System.IO;

namespace Ordering.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");
            var configuration = builder.Build();


            builder = new ConfigurationBuilder().AddApollo(builder.Build().GetSection("apollo"))
                .AddNamespace("spzl.shopdemo.ordering");
            var d = builder.Build();


            var appSetting = new ServiceCollection()
               .Configure<AppSettings>(builder.Build());

            var appConf = appSetting.BuildServiceProvider().GetService<IOptionsSnapshot<AppSettings>>();
            
            return Host.CreateDefaultBuilder(args)
                 //.ConfigureAppConfiguration((hostingContext, builder) =>
                 //{
                 //    builder
                 //    .AddApollo(builder.Build().GetSection("apollo"))
                 //       .AddNamespace("spzl.shopdemo.ordering");
                 //})
                  .ConfigureLogging((context, logBuilder) =>
                  {
                      //清空系统的框架自带的 log，要不然控制台输出很乱的
                      logBuilder.ClearProviders();
                      // 注入 log4日志
                      logBuilder.AddLog4Net();
                  })
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                      webBuilder.UseKestrel(options =>
                      {
                          options.Limits.MaxRequestBodySize = null;
                      });
                      // webBuilder.UseUrls($"http://{appConf.Value.DeployHostName}:{appConf.Value.DeployPort}");
                  });

        }

    }
}
