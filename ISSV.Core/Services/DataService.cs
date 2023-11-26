using ISSV.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ISSV.Core.Services
{
    public static class DataService
    {
        static DataService()
        {
            mySqlDataService = new MySqlDataService();
            EnsureCreated();
        }

        private static readonly MySqlDataService mySqlDataService;

        public static DbSet<Customer> Customers => mySqlDataService.Customers;

        public static DbSet<Location> Locations => mySqlDataService.Locations;

        public static DbSet<Device> Devices => mySqlDataService.Devices;

        public static DbSet<Maintenance> Maintenances => mySqlDataService.Maintenances;

        public static DbSet<Address> Addresses => mySqlDataService.Addresses;

        public static void EnsureCreated()
        {
            ((RelationalDatabaseCreator)mySqlDataService.Database.GetService<IDatabaseCreator>()).EnsureCreated();
        }

        public static void SaveChanges()
        {
            mySqlDataService.SaveChanges();
        }

        public static async Task SaveChangesAsync()
        {
            await mySqlDataService.SaveChangesAsync();
        }
    }
}
