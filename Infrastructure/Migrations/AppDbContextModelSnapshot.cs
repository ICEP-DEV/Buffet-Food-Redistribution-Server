﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminId");

                    b.ToTable("Admin");

                    b.HasData(
                        new
                        {
                            AdminId = 1,
                            Email = "admin@gmail.com",
                            Name = "admin",
                            Password = "admin",
                            Phone = "0126547380"
                        });
                });

            modelBuilder.Entity("Domain.Entities.DonationRequest", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<int>("DonationId")
                        .HasColumnType("int");

                    b.Property<int>("RecipientId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RequestId");

                    b.HasIndex("DonationId");

                    b.HasIndex("RecipientId");

                    b.ToTable("DonationRequests");
                });

            modelBuilder.Entity("Domain.Entities.Donor", b =>
                {
                    b.Property<int>("DonorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DonorId"));

                    b.Property<string>("DonorAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DonorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DonorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DonorPhoneNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("DonorId");

                    b.ToTable("Donors");

                    b.HasData(
                        new
                        {
                            DonorId = 1,
                            DonorAddress = "222 Servaas street",
                            DonorEmail = "kamomohapi17@gmail.com",
                            DonorName = "Kamohelo",
                            DonorPhoneNum = "0123456789",
                            Password = "$2a$11$o1L/HAkiHIB.e0AVGIoToevq5JkgrdfA5pF2cJTa5qSLJ5HU6RpNO"
                        },
                        new
                        {
                            DonorId = 2,
                            DonorAddress = "848 Motsi street",
                            DonorEmail = "tshepo@gmail.com",
                            DonorName = "Tshepo",
                            DonorPhoneNum = "0712563738",
                            Password = "$2a$11$o1L/HAkiHIB.e0AVGIoToevq5JkgrdfA5pF2cJTa5qSLJ5HU6RpNO"
                        },
                        new
                        {
                            DonorId = 3,
                            DonorAddress = "101 Linden street",
                            DonorEmail = "thabo@gmail.com",
                            DonorName = "Thabo",
                            DonorPhoneNum = "0812435627",
                            Password = "$2a$11$o1L/HAkiHIB.e0AVGIoToevq5JkgrdfA5pF2cJTa5qSLJ5HU6RpNO"
                        });
                });

            modelBuilder.Entity("Domain.Entities.FoodDonation", b =>
                {
                    b.Property<int>("DonationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DonationId"));

                    b.Property<DateTime>("DateCooked")
                        .HasColumnType("datetime2");

                    b.Property<int>("DonorId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("DonationId");

                    b.HasIndex("DonorId");

                    b.HasIndex("ItemId");

                    b.ToTable("FoodDonations");
                });

            modelBuilder.Entity("Domain.Entities.FoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCooked")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("Domain.Entities.Recipient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RecipientPhoneNum")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Recipients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "$2a$11$o1L/HAkiHIB.e0AVGIoToevq5JkgrdfA5pF2cJTa5qSLJ5HU6RpNO",
                            RecipientAddress = "191 Frederick street",
                            RecipientEmail = "kamomohapi17@gmail.com",
                            RecipientName = "Lesedi",
                            RecipientPhoneNum = "0193377233"
                        },
                        new
                        {
                            Id = 2,
                            Password = "$2a$11$o1L/HAkiHIB.e0AVGIoToevq5JkgrdfA5pF2cJTa5qSLJ5HU6RpNO",
                            RecipientAddress = "1921 Maltzan street",
                            RecipientEmail = "karabo@gmail.com",
                            RecipientName = "Karabo",
                            RecipientPhoneNum = "0135537733"
                        });
                });

            modelBuilder.Entity("Domain.Entities.DonationRequest", b =>
                {
                    b.HasOne("Domain.Entities.FoodDonation", "Donation")
                        .WithMany()
                        .HasForeignKey("DonationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Recipient", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Donation");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Domain.Entities.FoodDonation", b =>
                {
                    b.HasOne("Domain.Entities.Donor", "Donor")
                        .WithMany()
                        .HasForeignKey("DonorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.FoodItem", "FoodItem")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Donor");

                    b.Navigation("FoodItem");
                });
#pragma warning restore 612, 618
        }
    }
}
