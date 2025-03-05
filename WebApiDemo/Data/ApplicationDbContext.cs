using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

namespace WebApiDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {


        }
        public DbSet<Shirt> Shirts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //data seeding
            modelBuilder.Entity<Shirt>().HasData(
                new Shirt { ShirtId = 1, Brand = "Nike", Color = "Red", Gender = "Men", Size = 10, Price = 10 },
                new Shirt { ShirtId = 2, Brand = "Adidas", Color = "Blue", Gender = "Men", Size = 8, Price = 10 },
                new Shirt { ShirtId = 3, Brand = "Puma", Color = "Green", Gender = "Women", Size = 6, Price = 10 },
                new Shirt { ShirtId = 4, Brand = "Reebok", Color = "Yellow", Gender = "Women", Size = 4, Price = 10 }
                );
        }
    }
}
