using ISSV.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ISSV.Core.Services
{
    public class MySqlDataService : DbContext
    {
        public MySqlDataService(DbContextOptions options) : base (options)
        { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<Address> Addresses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
