using ISSV.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace ISSV.Core.Services
{
    public static class DataService
    {
        private static MySqlDataService mySqlDataService;

        public static DbSet<Customer> Customers => mySqlDataService.Customers;

        public static DbSet<Location> Locations => mySqlDataService.Locations;

        public static DbSet<Device> Devices => mySqlDataService.Devices;

        public static DbSet<Maintenance> Maintenances => mySqlDataService.Maintenances;

        public static DbSet<Address> Addresses => mySqlDataService.Addresses;

        public static bool IsConnected()
        {
            return mySqlDataService is MySqlDataService;
        }

        public static void Disconnect()
        {
            if (IsConnected())
            {
                mySqlDataService.Dispose();
                mySqlDataService = null;
            }
        }

        public static async Task<bool> ConnectAsync(string username, string password)
        {
            var conn = new MySqlConnectionStringBuilder
            {
                UserID = username,
                Password = password,
                Port = 3306,
                Server = "192.168.1.254",
                Database = "issv",
                SslMode = MySqlSslMode.None,
                AllowPublicKeyRetrieval = true,
            };
            try
            {
                var options = new DbContextOptionsBuilder<MySqlDataService>().UseMySql(conn.ConnectionString).Options;
                var service = new MySqlDataService(options);
                if (await service.Database.CanConnectAsync())
                {
                    mySqlDataService = service;
                    await EnsureCreatedAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task EnsureCreatedAsync()
        {
            await ((RelationalDatabaseCreator)mySqlDataService.Database.GetService<IDatabaseCreator>()).EnsureCreatedAsync();
        }

        public static void SaveChanges()
        {
            mySqlDataService.SaveChanges();
        }

        public static async Task SaveChangesAsync()
        {
            await mySqlDataService.SaveChangesAsync();
        }

        public static async Task<bool> CanConnect()
        {
            return await mySqlDataService.Database.CanConnectAsync();
        }
    }
}
