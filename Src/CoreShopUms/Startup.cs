using CoreShopUms.Conf;
using CoreShopUms.Handler;
using DotNetCore.CAP.Dashboard.NodeDiscovery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreShopUms
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
            // ӳ�������ļ�
            services.Configure<AppSettings>(Configuration);



            var appConf = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();

            // ���� Cap 
            services.AddCap(x =>
            {
                // �������ݿ�
                x.UseMySql(opt =>
                {
                    opt.ConnectionString = appConf.Value.Cap.ConnectionString;
                    opt.TableNamePrefix = appConf.Value.Cap.TableNamePrefix;
                });

                // ������Ϣ����
                x.UseRabbitMQ(opt =>
                {
                    opt.HostName = appConf.Value.RabbitMq.HostName;
                    opt.UserName = appConf.Value.RabbitMq.UserName;
                    opt.Password = appConf.Value.RabbitMq.PassWord;
                });

                // ����cap dashboard
                x.UseDashboard();

                // ע�ᵽconsul

                // ע��ڵ㵽 Consul
                x.UseDiscovery(d =>
                {
                    //d.DiscoveryServerHostName = "localhost";
                    //d.DiscoveryServerPort = 8500;
                    //d.CurrentNodeHostName = "localhost";
                    //d.CurrentNodePort = 5800;
                    //d.NodeId = 1;
                    //d.NodeName = "CAP No.1 Node";

                });
            });

            



            // ע��cap ����
            services.AddTransient<TestEventHandler>();


            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSettings> settings)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
