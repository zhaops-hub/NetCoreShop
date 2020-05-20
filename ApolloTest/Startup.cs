using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApolloTest.Conf;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApolloTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //IOptions<T> //站点启动后，获取到的值永远不变


            //IOptionsSnapshot<T> //站点启动后，每次获取到的值都是配置文件里的最新值 （reloadOnChange:true 必须为true）


            //IOptionsMonitor<T> //站点启动后，如果配置文件有变化会发布事件 （reloadOnChange:true 必须为true）


            services.Configure<AppSettings>(Configuration);


            var appConf = services.BuildServiceProvider().GetService<IOptionsMonitor<AppSettings>>();

            appConf.OnChange((listener) =>
            {
                // 配置文件更新了
                Console.WriteLine(" 配置文件更新了 ");

            });



            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
