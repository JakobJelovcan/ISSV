using ISSV.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace ISSV.Core.Services
{
    // This class holds sample data used by some generated pages to show how they can be used.
    // More information on using and configuring this service can be found at https://github.com/microsoft/TemplateStudio/blob/main/docs/UWP/services/sql-server-data-service.md
    // TODO: Change your code to use this instead of the SampleDataService.
    public static class SqlServerDataService
    {
        // TODO: Specify the connection string in a config file or below.
        private static string GetConnectionString()
        {
            // Attempt to get the connection string from a config file
            // Learn more about specifying the connection string in a config file at https://docs.microsoft.com/dotnet/api/system.configuration.configurationmanager?view=netframework-4.7.2
            var conStr = ConfigurationManager.ConnectionStrings["MyAppConnectionString"]?.ConnectionString;

            if (!string.IsNullOrWhiteSpace(conStr))
            {
                return conStr;
            }
            else
            {
                // If no connection string is specified in a config file, use this as a fallback.
                return @"Data Source=.\SQLEXPRESS;Database=northwind;Integrated Security=true;Trusted_Connection=true";
            }
        }


        public static async Task<IEnumerable<Customer>> GetAllDataAsync()
        {
            const string getDataQuery = @"SELECT
                                        dbo.Customer.Id,
                                        dbo.Customer.Name,
                                        dbo.Customer.PhoneNumber,
                                        dbo.Customer.Email,
                                        dbo.Customer.Address,
                                        dbo.Customer.Active,
                                        dbo.Location.Id,
                                        dbo.Location.Name,
                                        dbo.Location.Address,
                                        dbo.Location.PhoneNumber,
                                        dbo.Location.Email,
                                        dbo.Location.Active,
                                        dbo.Device.Id,
                                        dbo.Device.DeviceType,
                                        dbo.Device.SerialNumber,
                                        dbo.Device.Active,
                                        dbo.Device.MaintenanceFrequency,
                                        dbo.Device.WarrantyPerion,
                                        dbo.Device.InstallationDate,
                                        dbo.Maintenance.Id,
                                        dbo.Maintenance.Date,
                                        dbo.Maintenance.Reason,
                                        dbo.Maintenance.WorkDone,
                                        dbo.Maintenance.Notes,
                                        dbo.Maintenance.WorkOrder,
                                        dbo.Maintenance.Repairman,
                                        dbo.Maintenance.RegularMaintenance
                                        FROM dbo.Customer
                                        inner join dbo.Location on dbo.Customer.Id = dbo.Location.CustomerId
                                        inner join dbo.Device on dbo.Location.Id = dbo.Device.LocationId
                                        inner join dbo.Maintenance on dbo.Device.Id = dbo.Maintenance.DeviceId;";

            var customers = new List<Customer>();
            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getDataQuery;
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var customerId = reader.GetInt32(0);
                                    var customer = customers.FirstOrDefault(c => c.Id == customerId);
                                    if (customer == null)
                                    {
                                        var name = reader.GetString(1);
                                        var phone = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
                                        var email = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty;
                                        var address = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
                                        var active = reader.GetBoolean(5);
                                        customers.Add(new Customer(customerId, name, phone, email, address, active));
                                    }

                                    var locationId = reader.GetInt32(6);
                                    var location = customer.Locations.FirstOrDefault(l => l.Id == locationId);
                                    if (location == null)
                                    {
                                        var name = reader.GetString(7);
                                        var address = reader.GetString(8);
                                        var phoneNumber = !reader.IsDBNull(9) ? reader.GetString(9) : string.Empty;
                                        var email = !reader.IsDBNull(10) ? reader.GetString(10) : string.Empty;
                                        var active = reader.GetBoolean(11);
                                        customer.Locations.Add(new Location(locationId, customer.Id, name, address, phoneNumber, email, active));
                                    }

                                    var deviceId = reader.GetInt32(12);
                                    var device = location.Devices.FirstOrDefault(d => d.Id == deviceId);
                                    if (device == null)
                                    {
                                        var deviceType = reader.GetString(13);
                                        var serialNumber = reader.GetString(14);
                                        var active = reader.GetBoolean(15);
                                        var maintenanceFrequency = reader.GetDateTimeOffset(16);
                                        var warrantyPeriod = reader.GetDateTimeOffset(17);
                                        var installationDate = reader.GetDateTime(18);
                                        location.Devices.Add(new Device(deviceId, location.Id, deviceType, serialNumber, active, maintenanceFrequency, warrantyPeriod, installationDate));
                                    }

                                    var maintenanceId = reader.GetInt32(19);
                                    var maintenance = device.Maintenances.FirstOrDefault(m => m.Id == maintenanceId);
                                    if (maintenance == null)
                                    {
                                        var date = reader.GetDateTime(20);
                                        var reason = reader.GetString(21);
                                        var workDone = reader.GetString(22);
                                        var notes = !reader.IsDBNull(23) ? reader.GetString(23) : string.Empty;
                                        var workOrder = reader.GetString(24);
                                        var repairman = reader.GetString(25);
                                        var regularMaintenance = reader.GetBoolean(26);
                                        device.Maintenances.Add(new Maintenance(maintenanceId, device.Id, date, reason, workDone, notes, workOrder, repairman, regularMaintenance));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return customers;
        }

        public static async Task<IEnumerable<string>> GetAllAddressesAsync()
        {
            //const string getDataQuery = @"SELECT
            //                            dbo.Customer.Address
            //                            FROM dbo.Customer
            //                            UNION
            //                            SELECT
            //                            dbo.Location.Address,
            //                            FROM dbo.Location;";

            const string getDataQuery = @"SELECT
                                        dbo.Customers.Address,
                                        dbo.Customers.City,
                                        dbo.Customers.Country
                                        FROM dbo.Customers;";

            var addresses = new List<string>();
            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getDataQuery;
                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    //var address = reader.GetString(0);
                                    //addresses.Add(address);
                                    var address = reader.GetString(0);
                                    var city = reader.GetString(1);
                                    var country = reader.GetString(2);
                                    addresses.Add($"{address}, {city}, {country}");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return addresses;
        }

        public static async Task UpdateCustomer(Customer customer)
        {
            const string updateCustomerQuery = @"UPDATE dbo.Customer SET
                                                 Name=@Name,
                                                 PhoneNumber=@PhoneNumber,
                                                 Email=@Email,
                                                 Address=@Address,
                                                 Active=@Active
                                                 WHERE
                                                 Id=@Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = updateCustomerQuery;
                            cmd.Parameters.AddWithValue("@Id", customer.Id);
                            cmd.Parameters.AddWithValue("@Name", customer.Name);
                            cmd.Parameters.AddWithValue("@PhoneNumber", customer.PhoneNumber);
                            cmd.Parameters.AddWithValue("@Email", customer.Email);
                            cmd.Parameters.AddWithValue("@Address", customer.Address);
                            cmd.Parameters.AddWithValue("@Active", customer.Active);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task UpdateLocation(Location location)
        {
            const string updateLocationQuery = @"UPDATE dbo.Location SET
                                                 Name=@Name,
                                                 Address=@Address,
                                                 PhoneNumber=@PhoneNumber,
                                                 Email=@Email,
                                                 Active=@Active
                                                 WHERE
                                                 Id=@Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = updateLocationQuery;
                            cmd.Parameters.AddWithValue("@Id", location.Id);
                            cmd.Parameters.AddWithValue("@Name", location.Name);
                            cmd.Parameters.AddWithValue("@Address", location.Address);
                            cmd.Parameters.AddWithValue("@PhoneNumber", location.PhoneNumber);
                            cmd.Parameters.AddWithValue("@Email", location.Email);
                            cmd.Parameters.AddWithValue("@Active", location.Active);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task UpdateDevice(Device device)
        {
            const string updateDeviceQuery = @"UPDATE dbo.Device SET
                                               DeviceType=@DeviceType,
                                               SerialNumber=@SerialNumber,
                                               Active=@Active,
                                               MaintenanceFrequency=@MaintenanceFrequency,
                                               WarrantyPeriod=@WarrantyPeriod,
                                               InstallationDate=@InstallationDate
                                               WHERE
                                               Id=@Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = updateDeviceQuery;
                            cmd.Parameters.AddWithValue("@Id", device.Id);
                            cmd.Parameters.AddWithValue("@DeviceType", device.DeviceType);
                            cmd.Parameters.AddWithValue("@SerialNumber", device.SerialNumber);
                            cmd.Parameters.AddWithValue("@Active", device.Active);
                            cmd.Parameters.AddWithValue("@MaintenanceFrequency", device.MaintenanceFrequency);
                            cmd.Parameters.AddWithValue("@WarrantyPeriod", device.WarrantyPeriod);
                            cmd.Parameters.AddWithValue("@InstallationDate", device.InstallationDate);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task UpdateMaintenance(Maintenance maintenance)
        {
            const string updateMaintenanceQuery = @"UPDATE dbo.Maintenance SET
                                                    Date=@Date,
                                                    Reason=@Reason,
                                                    WorkDone=@WorkDone,
                                                    Notes=@Notes,
                                                    WorkOrder=@WorkOrder,
                                                    Repairman=@Repairman,
                                                    RegularMaintenance=@RegularMaintenance
                                                    WHERE
                                                    Id=@Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = updateMaintenanceQuery;
                            cmd.Parameters.AddWithValue("@Id", maintenance.Id);
                            cmd.Parameters.AddWithValue("@Date", maintenance.Date);
                            cmd.Parameters.AddWithValue("@Reason", maintenance.Reason);
                            cmd.Parameters.AddWithValue("@WorkDone", maintenance.WorkDone);
                            cmd.Parameters.AddWithValue("@WorkOrder", maintenance.WorkOrder);
                            cmd.Parameters.AddWithValue("@Note", maintenance.Notes);
                            cmd.Parameters.AddWithValue("@Repairman", maintenance.Repairman);
                            cmd.Parameters.AddWithValue("@RegularMaintenance", maintenance.RegularMaintenance);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task<Customer> InsertCustomer(string name, string phoneNumber, string email, string address, bool active)
        {
            const string insertCustomerQuery = @"INSERT INTO dbo.Customer (
                                                 Name,
                                                 PhoneNumber,
                                                 Email,
                                                 Address,
                                                 Active
                                                 ) VALUES (                                                 
                                                 @Name,
                                                 @PhoneNumber,
                                                 @Email,
                                                 @Address,
                                                 @Active);";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = insertCustomerQuery;
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@Active", active);

                            int id = (int)await cmd.ExecuteScalarAsync();
                            return new Customer(id, name, phoneNumber, email, address, active);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return null;
        }

        public static async Task<Location> InsertLocation(int customerId, string name, string address, string phoneNumber, string email, bool active)
        {
            const string insertLocationQuery = @"INSERT INTO dbo.Location (
                                                 CustomerId,
                                                 Name,
                                                 Address,
                                                 PhoneNumber,
                                                 Email,
                                                 Active
                                                 ) VALUES (
                                                 @CustomerId,
                                                 @Name,
                                                 @Address,
                                                 @PhoneNumber,
                                                 @Email,
                                                 @Active);";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = insertLocationQuery;
                            cmd.Parameters.AddWithValue("@CustomerId", customerId);
                            cmd.Parameters.AddWithValue("@Name", name);
                            cmd.Parameters.AddWithValue("@Address", address);
                            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Active", active);

                            int id = (int)await cmd.ExecuteScalarAsync();
                            return new Location(id, customerId, name, address, phoneNumber, email, active);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return null;
        }

        public static async Task<Device> InsertDevice(int locationId, string deviceType, string serialNumber, bool active, DateTimeOffset maintenanceFrequency, DateTimeOffset warrantyPeriod, DateTime installationDate)
        {
            const string insertDeviceQuery = @"INSERT INTO dbo.Device (
                                               LocationId,
                                               DeviceType,
                                               SerialNumber,
                                               Active,
                                               MaintenanceFrequency,
                                               WarrantyPeriod,
                                               InstallationDate
                                               ) VALUES (
                                               @LocationId,
                                               @DeviceType,
                                               @SerialNumber,
                                               @Active,
                                               @MaintenanceFrequency,
                                               @WarrantyPeriod,
                                               @InstallationDate);";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = insertDeviceQuery;
                            cmd.Parameters.AddWithValue("@LocationId", locationId);
                            cmd.Parameters.AddWithValue("@DeviceType", deviceType);
                            cmd.Parameters.AddWithValue("@SerialNumber", serialNumber);
                            cmd.Parameters.AddWithValue("@Active", active);
                            cmd.Parameters.AddWithValue("@MaintenanceFrequency", maintenanceFrequency);
                            cmd.Parameters.AddWithValue("@WarrantyPeriod", warrantyPeriod);
                            cmd.Parameters.AddWithValue("@InstallationDate", installationDate);

                            int id = (int)await cmd.ExecuteScalarAsync();
                            return new Device(id, locationId, deviceType, serialNumber, active, maintenanceFrequency, warrantyPeriod, installationDate);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return null;
        }

        public static async Task<Maintenance> InsertMaintenance(int deviceId, DateTime date, string reason, string workDone, string notes, string workOrder, string repairman, bool regularMaintenance)
        {
            const string insertMaintenanceQuery = @"INSERT ITEM dbo.Maintenance (
                                                    DeviceId,
                                                    Date,
                                                    Reason,
                                                    WorkDone,
                                                    Notes,
                                                    WorkOrder,
                                                    RepairMan,
                                                    RegularMaintenance
                                                    ) VALUES (
                                                    @DeviceId,
                                                    @Date,
                                                    @Reason,
                                                    @WorkDone,
                                                    @Notes,
                                                    @WorkOrder,
                                                    @Repairman,
                                                    @RegularMaintenance);";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = insertMaintenanceQuery;
                            cmd.Parameters.AddWithValue("@DeviceId", deviceId);
                            cmd.Parameters.AddWithValue("@Date", date);
                            cmd.Parameters.AddWithValue("@Reason", reason);
                            cmd.Parameters.AddWithValue("@WorkDone", workDone);
                            cmd.Parameters.AddWithValue("@WorkOrder", workOrder);
                            cmd.Parameters.AddWithValue("@Note", notes);
                            cmd.Parameters.AddWithValue("@Repairman", repairman);
                            cmd.Parameters.AddWithValue("@RegularMaintenance", regularMaintenance);

                            int id = (int)await cmd.ExecuteScalarAsync();
                            return new Maintenance(id, deviceId, date, reason, workDone, notes, workOrder, repairman, regularMaintenance);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
            return null;
        }

        public static async Task DeleteCustomer(int id)
        {
            const string deleteCustomerQuery = @"DELETE FROM dbo.Customer WHERE Id = @Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = deleteCustomerQuery;
                            cmd.Parameters.AddWithValue("@Id", id);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task DeleteLocation(int id)
        {
            const string deleteCustomerQuery = @"DELETE FROM dbo.Location WHERE Id = @Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = deleteCustomerQuery;
                            cmd.Parameters.AddWithValue("@Id", id);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task DeleteDevice(int id)
        {
            const string deleteCustomerQuery = @"DELETE FROM dbo.Device WHERE Id = @Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = deleteCustomerQuery;
                            cmd.Parameters.AddWithValue("@Id", id);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }

        public static async Task DeleteMaintenance(int id)
        {
            const string deleteCustomerQuery = @"DELETE FROM dbo.Maintenance WHERE Id = @Id;";

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = deleteCustomerQuery;
                            cmd.Parameters.AddWithValue("@Id", id);

                            await cmd.ExecuteNonQueryAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message} {ex.InnerException?.Message}");
            }
        }


        public static async Task<IEnumerable<SampleOrder>> AllOrders()
        {
            // List orders from all companies
            var companies = await AllCompanies();
            return companies.SelectMany(c => c.Orders);
        }

        // This method returns data with the same structure as the SampleDataService but based on the NORTHWIND sample database.
        // Use this as an alternative to the sample data to test using a different datasource without changing any other code.
        // TODO: Remove this when or if it isn't needed.
        public static async Task<IEnumerable<SampleCompany>> AllCompanies()
        {
            // This hard-coded SQL statement is included to make this sample simpler.
            // You can use Stored procedure, ORMs, or whatever is appropriate to access data in your app.
            const string getSampleDataQuery = @"
            SELECT dbo.Customers.CustomerID,
                dbo.Customers.CompanyName,
                dbo.Customers.ContactName,
                dbo.Customers.ContactTitle,
                dbo.Customers.Address,
                dbo.Customers.City,
                dbo.Customers.PostalCode,
                dbo.Customers.Country,
                dbo.Customers.Phone,
                dbo.Customers.Fax,
                dbo.Orders.OrderID,
                dbo.Orders.OrderDate,
                dbo.Orders.RequiredDate,
                dbo.Orders.ShippedDate,
                dbo.Orders.Freight,
                dbo.Shippers.CompanyName,
                dbo.Shippers.Phone,
                CONCAT(dbo.Customers.Address, ' ', dbo.Customers.City, ' ', dbo.Customers.PostalCode, ' ', dbo.Customers.Country) as ShipTo,
                ISNULL(CHOOSE(CAST(RAND(CHECKSUM(NEWID())) * 3 as INT), 'Shipped', 'Closed'), 'New') as Status,
                CAST(RAND(CHECKSUM(NEWID())) * 200 as INT) + 57600 as SymbolCode,
                SUM(dbo.[Order Details].UnitPrice * dbo.[Order Details].Quantity * (1 + dbo.[Order Details].Discount)) OVER(PARTITION BY Orders.OrderID) AS OrderTotal,
                dbo.Products.ProductID,
                dbo.Products.ProductName,
                dbo.[Order Details].Quantity,
                dbo.[Order Details].Discount,
                dbo.[Order Details].UnitPrice,
                dbo.Products.QuantityPerUnit,
                dbo.Categories.CategoryName,
                dbo.Categories.Description,
                dbo.[Order Details].UnitPrice * dbo.[Order Details].Quantity * (1 + dbo.[Order Details].Discount) as ProductTotal
            FROM dbo.Customers
                inner join dbo.Orders on dbo.Customers.CustomerID = dbo.Orders.CustomerID
                inner join dbo.[Order Details] on dbo.[Order Details].OrderID = dbo.Orders.OrderID
                inner join dbo.Shippers on dbo.Orders.ShipVia = dbo.Shippers.ShipperID
                inner join dbo.Products on dbo.Products.ProductID = dbo.[Order Details].ProductID
                inner join dbo.Categories on dbo.Categories.CategoryID = dbo.Products.CategoryID";

            var sampleCompanies = new List<SampleCompany>();

            try
            {
                using (var conn = new SqlConnection(GetConnectionString()))
                {
                    await conn.OpenAsync();

                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        using (var cmd = conn.CreateCommand())
                        {
                            cmd.CommandText = getSampleDataQuery;

                            using (var reader = await cmd.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    // Company Data
                                    var companyID = reader.GetString(0);
                                    var companyName = reader.GetString(1);
                                    var sampleCompany = sampleCompanies.FirstOrDefault(c => c.CompanyID == companyID);
                                    if (sampleCompany == null)
                                    {
                                        var contactName = !reader.IsDBNull(2) ? reader.GetString(2) : string.Empty;
                                        var contactTitle = !reader.IsDBNull(3) ? reader.GetString(3) : string.Empty;
                                        var address = !reader.IsDBNull(4) ? reader.GetString(4) : string.Empty;
                                        var city = !reader.IsDBNull(5) ? reader.GetString(5) : string.Empty;
                                        var postalCode = !reader.IsDBNull(6) ? reader.GetString(6) : string.Empty;
                                        var country = !reader.IsDBNull(7) ? reader.GetString(7) : string.Empty;
                                        var phone = !reader.IsDBNull(8) ? reader.GetString(8) : string.Empty;
                                        var fax = !reader.IsDBNull(9) ? reader.GetString(9) : string.Empty;

                                        sampleCompany = new SampleCompany()
                                        {
                                            CompanyID = companyID,
                                            CompanyName = companyName,
                                            ContactName = contactName,
                                            ContactTitle = contactTitle,
                                            Address = address,
                                            City = city,
                                            PostalCode = postalCode,
                                            Country = country,
                                            Phone = phone,
                                            Fax = fax,
                                            Orders = new List<SampleOrder>()
                                        };
                                        sampleCompanies.Add(sampleCompany);
                                    }

                                    // Order Data
                                    var orderID = reader.GetInt32(10);
                                    var sampleOrder = sampleCompany.Orders.FirstOrDefault(o => o.OrderID == orderID);
                                    if (sampleOrder == null)
                                    {
                                        var orderDate = !reader.IsDBNull(11) ? reader.GetDateTime(11) : default;
                                        var requiredDate = !reader.IsDBNull(12) ? reader.GetDateTime(12) : default;
                                        var shippedDate = !reader.IsDBNull(13) ? reader.GetDateTime(13) : default;
                                        var freight = !reader.IsDBNull(14) ? double.Parse(reader.GetDecimal(14).ToString()) : 0;
                                        var shipperName = !reader.IsDBNull(15) ? reader.GetString(15) : string.Empty;
                                        var shipperPhone = !reader.IsDBNull(16) ? reader.GetString(16) : string.Empty;
                                        var shipTo = !reader.IsDBNull(17) ? reader.GetString(17) : string.Empty;
                                        var status = !reader.IsDBNull(18) ? reader.GetString(18) : string.Empty;
                                        var symbolCode = !reader.IsDBNull(19) ? reader.GetInt32(19) : 0;
                                        var orderTotal = !reader.IsDBNull(29) ? reader.GetDouble(20) : 0.0;

                                        sampleOrder = new SampleOrder()
                                        {
                                            OrderID = orderID,
                                            OrderDate = orderDate,
                                            RequiredDate = requiredDate,
                                            ShippedDate = shippedDate,
                                            ShipperName = shipperName,
                                            ShipperPhone = shipperPhone,
                                            Freight = freight,
                                            Company = companyName,
                                            ShipTo = shipTo,
                                            Status = status,
                                            SymbolCode = symbolCode,
                                            OrderTotal = orderTotal,
                                            Details = new List<SampleOrderDetail>()
                                        };
                                        sampleCompany.Orders.Add(sampleOrder);
                                    }

                                    // Product Data
                                    var productID = reader.GetInt32(21);
                                    var productName = reader.GetString(22);
                                    var quantity = reader.GetInt16(23);
                                    var discount = reader.GetFloat(24);
                                    var unitPrice = double.Parse(reader.GetDecimal(25).ToString());
                                    var quantityPerUnit = !reader.IsDBNull(26) ? reader.GetString(26) : string.Empty;
                                    var categoryName = !reader.IsDBNull(27) ? reader.GetString(27) : string.Empty;
                                    var categoryDescription = !reader.IsDBNull(28) ? reader.GetString(28) : string.Empty;
                                    var productTotal = !reader.IsDBNull(29) ? reader.GetFloat(29) : 0.0;

                                    sampleOrder.Details.Add(new SampleOrderDetail()
                                    {
                                        ProductID = productID,
                                        ProductName = productName,
                                        Quantity = quantity,
                                        Discount = discount,
                                        QuantityPerUnit = quantityPerUnit,
                                        UnitPrice = unitPrice,
                                        CategoryName = categoryName,
                                        CategoryDescription = categoryDescription,
                                        Total = productTotal
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eSql)
            {
                // Your code may benefit from more robust error handling or logging.
                // This logging is just a reminder that you should handle exceptions when connecting to remote data.
                System.Diagnostics.Debug.WriteLine($"Exception: {eSql.Message} {eSql.InnerException?.Message}");
            }

            return sampleCompanies;
        }
    }
}
