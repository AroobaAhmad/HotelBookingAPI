using HotelBookingAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToContainer("hotelcustomers") // cosmos DB container name
                .HasPartitionKey<Customer, object>(c => c.category).HasNoDiscriminator();
            modelBuilder.Entity<Customer>().HasKey(c => c.id);
        }
    }
}
