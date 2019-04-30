﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Database;

namespace WebApi.Database.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity("WebApi.Database.Entities.AddressCityEty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountyId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Citys");
                });

            modelBuilder.Entity("WebApi.Database.Entities.AddressCountyEty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Initials")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(2);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Countys");
                });

            modelBuilder.Entity("WebApi.Database.Entities.AddressDistrictEty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CityId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("WebApi.Database.Entities.AddressEty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CityId");

                    b.Property<string>("CityName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("CountyId");

                    b.Property<string>("CountyInitials")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(2);

                    b.Property<string>("CountyName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("DistrictId");

                    b.Property<string>("DistrictName")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("ZipCode")
                        .IsFixedLength(true)
                        .HasMaxLength(8);

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("WebApi.Database.Entities.ClientEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<int?>("AddressId");

                    b.Property<string>("ContactName")
                        .HasMaxLength(60);

                    b.Property<string>("CpfCnpj")
                        .HasMaxLength(14);

                    b.Property<DateTimeOffset>("CreateAt");

                    b.Property<string>("Email")
                        .HasMaxLength(60);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Org")
                        .HasMaxLength(20);

                    b.Property<string>("Phone")
                        .IsFixedLength(true)
                        .HasMaxLength(11);

                    b.Property<string>("RgIE")
                        .HasMaxLength(20);

                    b.HasKey("CompanyId", "Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CompanyId", "CpfCnpj")
                        .IsUnique();

                    b.HasIndex("CompanyId", "Email")
                        .IsUnique();

                    b.HasIndex("CompanyId", "Phone")
                        .IsUnique();

                    b.HasIndex("CompanyId", "RgIE")
                        .IsUnique();

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("WebApi.Database.Entities.CompanyEty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AddressId");

                    b.Property<string>("CpfCnpj")
                        .IsRequired()
                        .HasMaxLength(14);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("LogoPath")
                        .HasMaxLength(300);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(11);

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CpfCnpj")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("WebApi.Database.Entities.ContextSequenceEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<string>("Context")
                        .HasMaxLength(20);

                    b.Property<int>("Value");

                    b.HasKey("CompanyId", "Context");

                    b.ToTable("ContextSequence");
                });

            modelBuilder.Entity("WebApi.Database.Entities.OrderDetailEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<decimal>("Honorary");

                    b.Property<int>("OrderId");

                    b.Property<decimal>("Other");

                    b.Property<decimal>("PlateCard");

                    b.Property<decimal>("Rate");

                    b.Property<int>("ServiceId");

                    b.Property<decimal>("Total");

                    b.Property<int>("VehicleId");

                    b.HasKey("CompanyId", "Id");

                    b.HasIndex("CompanyId", "OrderId");

                    b.HasIndex("CompanyId", "ServiceId");

                    b.HasIndex("CompanyId", "VehicleId");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("WebApi.Database.Entities.OrderServiceEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<int>("ClientId");

                    b.Property<DateTimeOffset?>("ClosedAt");

                    b.Property<DateTimeOffset>("CreateAt");

                    b.Property<int>("Status");

                    b.Property<decimal>("Total");

                    b.HasKey("CompanyId", "Id");

                    b.HasIndex("CompanyId", "ClientId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("WebApi.Database.Entities.ServiceEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<decimal>("Honorary");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<decimal>("Other");

                    b.Property<decimal>("PlateCard");

                    b.Property<decimal>("Rate");

                    b.HasKey("CompanyId", "Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("WebApi.Database.Entities.UserEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("CompanyId", "Id");

                    b.HasIndex("CompanyId", "Email")
                        .IsUnique();

                    b.HasIndex("Email", "Password");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WebApi.Database.Entities.VehicleEty", b =>
                {
                    b.Property<int>("CompanyId");

                    b.Property<int>("Id");

                    b.Property<string>("Chassis")
                        .IsFixedLength(true)
                        .HasMaxLength(17);

                    b.Property<int?>("CityId");

                    b.Property<string>("CityName")
                        .HasMaxLength(200);

                    b.Property<int>("ClientId");

                    b.Property<string>("Color")
                        .HasMaxLength(20);

                    b.Property<int?>("CountyId");

                    b.Property<string>("CountyInitials")
                        .IsFixedLength(true)
                        .HasMaxLength(2);

                    b.Property<string>("CountyName")
                        .HasMaxLength(200);

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(60);

                    b.Property<string>("Model")
                        .HasMaxLength(60);

                    b.Property<int?>("ModelYear");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .IsFixedLength(true)
                        .HasMaxLength(7);

                    b.Property<string>("Renavam")
                        .IsFixedLength(true)
                        .HasMaxLength(11);

                    b.Property<int>("Type");

                    b.Property<int?>("YearManufacture");

                    b.HasKey("CompanyId", "Id");

                    b.HasIndex("Chassis")
                        .IsUnique();

                    b.HasIndex("CityId");

                    b.HasIndex("CountyId");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.HasIndex("Renavam")
                        .IsUnique();

                    b.HasIndex("CompanyId", "ClientId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("WebApi.Database.Entities.ClientEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.AddressEty", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Database.Entities.CompanyEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.AddressEty", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");
                });

            modelBuilder.Entity("WebApi.Database.Entities.ContextSequenceEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Database.Entities.OrderDetailEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.OrderServiceEty", "Order")
                        .WithMany("Items")
                        .HasForeignKey("CompanyId", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Database.Entities.ServiceEty", "Service")
                        .WithMany()
                        .HasForeignKey("CompanyId", "ServiceId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("WebApi.Database.Entities.VehicleEty", "Vehicle")
                        .WithMany()
                        .HasForeignKey("CompanyId", "VehicleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WebApi.Database.Entities.OrderServiceEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Database.Entities.ClientEty", "Client")
                        .WithMany()
                        .HasForeignKey("CompanyId", "ClientId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("WebApi.Database.Entities.ServiceEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Database.Entities.UserEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WebApi.Database.Entities.VehicleEty", b =>
                {
                    b.HasOne("WebApi.Database.Entities.AddressCityEty", "City")
                        .WithMany()
                        .HasForeignKey("CityId");

                    b.HasOne("WebApi.Database.Entities.CompanyEty", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WebApi.Database.Entities.AddressCountyEty", "County")
                        .WithMany()
                        .HasForeignKey("CountyId");

                    b.HasOne("WebApi.Database.Entities.ClientEty", "Client")
                        .WithMany()
                        .HasForeignKey("CompanyId", "ClientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}