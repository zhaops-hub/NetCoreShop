using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShopUms.Mappers
{
    public static class AutoMapperSetup
    {
        public static IServiceCollection AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            //添加服务
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //启动配置
            AutoMapperConfig.RegisterMappings();

            return services;
        }
    }
}
