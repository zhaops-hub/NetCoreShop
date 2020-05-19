using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ordering.Application.DomainEventHandlers.Order;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Conf;
using Ordering.Domain.Events;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using Ordering.Service.Application.Commands;

namespace Ordering.Service
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

            // 数据库
            services.AddDbContext<OrderingContext>((options) =>
            {
                options.UseMySQL(appConf.Value.DefaultConnection);
            }, ServiceLifetime.Scoped);

            services.AddMediatR(typeof(Startup));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRequestManager, RequestManager>();
            services.AddScoped<IRequestHandler<IdentifiedCommand, bool>, CreateOrderIdentifiedCommandHandler>();
            services.AddScoped<IRequestHandler<CreateOrderCommand, bool>, CreateOrderCommandHandler>();

            services.AddScoped<INotificationHandler<OrderStartedDomainEvent>, OrderStartedDomainEventHandler>();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
