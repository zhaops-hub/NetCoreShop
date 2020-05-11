using CoreShopUms.Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreShopUms.Infrastructure.Map
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.Id).HasColumnName("Id").HasColumnType("varchar(50)");
            builder.Property(c => c.Account).HasColumnType("varchar(50)");
            builder.Property(c => c.Password).HasColumnType("varchar(300)");
            builder.Property(c => c.RealName).HasColumnType("varchar(50)");
        }
    }
}