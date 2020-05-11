using CoreShopUms.Conf;
using CoreShopUms.Handler;
using CoreShopUms.Infrastructure;
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

                // ע��ڵ㵽 Consul
                x.UseDiscovery(d =>
                {
                    d.DiscoveryServerHostName = appConf.Value.Consul.HostName;
                    d.DiscoveryServerPort = appConf.Value.Consul.Port;
                    d.CurrentNodeHostName = appConf.Value.Deploy.HostName;
                    d.CurrentNodePort = appConf.Value.Deploy.Port;
                    d.NodeId = appConf.Value.Deploy.NodeId;
                    d.NodeName = appConf.Value.Deploy.NodeName;
                    d.MatchPath = "/Health";
                });
            });

            // ע��cap ����
            services.AddTransient<TestEventHandler>();

            services.AddScoped<UmsContext>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UmsContext umsContext)
        {
            app.UseRouting();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}