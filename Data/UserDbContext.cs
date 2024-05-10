using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options) { }

        //public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users => Set<User>();

    }

   
}
