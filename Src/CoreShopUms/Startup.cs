using CoreShopUms.Conf;
using CoreShopUms.Extensions;
using CoreShopUms.Handler;
using CoreShopUms.Infrastructure;
using CoreShopUms.Mappers;
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
            // 映射配置文件
            services.Configure<AppSettings>(Configuration);


            var appConf = services.BuildServiceProvider().GetService<IOptions<AppSettings>>(); 

            // 配置 Cap 
            services.AddCap(x =>
            {
                // 配置数据库
                x.UseMySql(opt =>
                {
                    opt.ConnectionString = appConf.Value.Cap.ConnectionString;
                    opt.TableNamePrefix = appConf.Value.Cap.TableNamePrefix;
                });

                // 配置消息队列
                x.UseRabbitMQ(opt =>
                {
                    opt.HostName = appConf.Value.RabbitMq.HostName;
                    opt.UserName = appConf.Value.RabbitMq.UserName;
                    opt.Password = appConf.Value.RabbitMq.PassWord;
                });

                // 配置cap dashboard
                x.UseDashboard();

                // 注册节点到 Consul
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

            // 注入cap 处理
            services.AddTransient<TestEventHandler>();

            // 配合 context
            services.AddScoped<UmsContext>();

            // 配置swaggerdoc
            services.AddSwagger();

            // autoMapper
            services.AddAutoMapperSetup(); 

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UmsContext umsContext)
        {
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ums api v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}