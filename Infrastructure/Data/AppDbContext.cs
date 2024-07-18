using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        { 
        
        }

       
        public DbSet<Donor> Donors { get; set; }
        public DbSet<FoodDonation>FoodDonations { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Admin> Admin { get; set; }

        public DbSet<FoodItem>FoodItems { get; set; }

        public DbSet<DonationRequest> DonationRequests { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword("password");

            modelBuilder.Entity<Donor>().HasData(

                new Donor { DonorId = 1, DonorName = "Kamohelo", DonorEmail = "kamomohapi17@gmail.com", DonorPhoneNum = "0123456789", DonorAddress = "222 Servaas street", Password = hashedPassword },
                new Donor { DonorId = 2, DonorName = "Tshepo", DonorEmail = "tshepo@gmail.com", DonorPhoneNum = "0712563738", DonorAddress = "848 Motsi street", Password = hashedPassword },
                new Donor { DonorId = 3, DonorName = "Thabo", DonorEmail = "thabo@gmail.com", DonorPhoneNum = "0812435627", DonorAddress = "101 Linden street", Password = hashedPassword }


            );


            modelBuilder.Entity<Recipient>().HasData(
                new Recipient
                {
                    Id = 1,
                    RecipientName = "Lesedi",
                    RecipientEmail = "kamomohapi17@gmail.com",
                    RecipientAddress = "191 Frederick street",
                    RecipientPhoneNum = "0193377233",
                    Password = hashedPassword

                },
                new Recipient
                {
                    Id = 2,
                    RecipientName = "Karabo",
                    RecipientEmail = "karabo@gmail.com",
                    RecipientAddress = "1921 Maltzan street",
                    RecipientPhoneNum = "0135537733",
                    Password = hashedPassword

                }
                );

            modelBuilder.Entity<Admin>().HasData(
                new  Admin 
                {
                    AdminId = 1,
                    Name = "admin",
                    Email = "admin@gmail.com",
                    Phone = "0126547380",
                    Password = "admin"
                }
                
                );
        }

    }
}
