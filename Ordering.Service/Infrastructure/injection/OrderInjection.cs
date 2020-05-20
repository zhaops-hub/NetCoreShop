﻿using DotNetCore.CAP.Dashboard.NodeDiscovery;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Ordering.Application.Commands;
using Ordering.Application.DomainEventHandlers.Order;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.Conf;
using Ordering.Domain.Events;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Idempotency;
using Ordering.Infrastructure.Repositories;
using System.Collections.Generic;
using System.IO;

namespace Ordering.Service.Infrastructure.injection
{
    public static class OrderInjection
    {
        public static IServiceCollection RegisterOrderService(this IServiceCollection services, IConfiguration configuration)
        {
            // 映射配置文件
            services.Configure<AppSettings>(configuration);
            var appConf = services.BuildServiceProvider().GetService<IOptions<AppSettings>>();


            // 数据库
            services.AddDbContext<OrderingContext>((options) =>
            {
                options.UseMySQL(appConf.Value.DefaultConnection);

            }, ServiceLifetime.Scoped);

            services.AddMediatR(typeof(IServiceCollection));

            // 仓库
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IRequestManager, RequestManager>();


            // 命令
            services.AddScoped<IRequestHandler<IdentifiedCommand, bool>, CreateOrderIdentifiedCommandHandler>();
            services.AddScoped<IRequestHandler<CreateOrderCommand, bool>, CreateOrderCommandHandler>();


            // 事件
            services.AddScoped<INotificationHandler<OrderStartedDomainEvent>, OrderStartedDomainEventHandler>();


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
                    //d.MatchPath = "/Health";
                });
            });


            // swaggerGen 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() { Title = " order api ", Version = "v1" });

                // 添加header验证
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } } };

                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization", //jwt默认的参数名称
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header
                });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath =
                    Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Ordering.Service.xml");
                var xmlPath1 = Path.Combine(basePath, "Ordering.Application.xml");
                c.IncludeXmlComments(xmlPath);
                c.IncludeXmlComments(xmlPath1);
            });

            return services;
        }
    }
}