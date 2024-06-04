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
    }
}
