using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Infrastructure.EntityConfigurations
{
    class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //builder.ToTable("orders");

            builder.HasKey(o => o.Id);
            
            builder.Ignore(b => b.DomainEvents);
            builder.Ignore(b => b.OrderStatus);
            builder.Ignore(b => b.OrderItems);

            builder
               .OwnsOne(o => o.Address, a =>
               {
                   a.WithOwner();
               });

            builder
               .Property<int?>("_buyerId")
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("BuyerId")
               .IsRequired(false);

            builder
               .Property<DateTime>("_orderDate")
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("OrderDate")
               .IsRequired();


            builder
               .Property<int?>("_orderStatusId")
               .HasColumnName("OrderStatusId")
               .IsRequired(false);

            builder
              .Property<int?>("_paymentMethodId")
              .UsePropertyAccessMode(PropertyAccessMode.Field)
              .HasColumnName("PaymentMethodId")
              .IsRequired(false);


            builder.Property<string>("Description").IsRequired(false);
        }
    }
}
