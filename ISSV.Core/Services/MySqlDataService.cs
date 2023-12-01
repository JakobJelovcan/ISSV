using ISSV.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ISSV.Core.Services
{
    public class MySqlDataService : DbContext
    {
        public MySqlDataService() { }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Device> Devices { get; set; }

        public DbSet<Maintenance> Maintenances { get; set; }

        public DbSet<Address> Addresses { get; set; }

        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MySqlConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"server=192.168.1.254;database=issv;userid=jakob;pwd=bAFUrJ7XZU;port=3306;sslmode=none;";
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString()).EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
