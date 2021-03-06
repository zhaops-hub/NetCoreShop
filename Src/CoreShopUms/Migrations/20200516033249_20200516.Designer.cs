﻿// <auto-generated />
using CoreShopUms.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreShopUms.Migrations
{
    [DbContext(typeof(UmsContext))]
    [Migration("20200516033249_20200516")]
    partial class _20200516
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CoreShopUms.Infrastructure.Entity.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("Id")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Account")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Password")
                        .HasColumnType("varchar(300)");

                    b.Property<string>("RealName")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
