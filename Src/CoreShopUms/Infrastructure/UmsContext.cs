using System.IO;
using CoreShopUms.Conf;
using CoreShopUms.Infrastructure.Entity;
using CoreShopUms.Infrastructure.Map;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CoreShopUms.Infrastructure
{
    public class UmsContext : DbContext
    {
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //对 StudentMap 进行配置
            modelBuilder.ApplyConfiguration(new UserMap());


            base.OnModelCreating(modelBuilder);
        }


        /// <summary>
        ///     重写连接数据库
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");


            var configuration = builder.Build();
            var appSetting = new ServiceCollection()
                .Configure<AppSettings>(configuration);

            var appConf = appSetting.BuildServiceProvider().GetService<IOptions<AppSettings>>();

            optionsBuilder.UseMySQL(appConf.Value.DefaultConnection);
        }
    }
}