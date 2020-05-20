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

            //IOptions<T> //վ�������󣬻�ȡ����ֵ��Զ����


            //IOptionsSnapshot<T> //վ��������ÿ�λ�ȡ����ֵ���������ļ��������ֵ ��reloadOnChange:true ����Ϊtrue��


            //IOptionsMonitor<T> //վ����������������ļ��б仯�ᷢ���¼� ��reloadOnChange:true ����Ϊtrue��


            services.Configure<AppSettings>(Configuration);


            var appConf = services.BuildServiceProvider().GetService<IOptionsMonitor<AppSettings>>();

            appConf.OnChange((listener) =>
            {
                // �����ļ�������
                Console.WriteLine(" �����ļ������� ");

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
