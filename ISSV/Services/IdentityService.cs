using ISSV.Core.Services;
using System;
using System.Threading.Tasks;

namespace ISSV.Services
{
    public class IdentityService
    {
        public event EventHandler LoggedIn;

        public async Task<bool> LogInAsync(string username, string password)
        {
            var status = await DataService.ConnectAsync(username, password);
            if (status)
            {
                LoggedIn?.Invoke(this, EventArgs.Empty);
            }
            return status;
        }

        public bool IsLoggedIn()
        {
            return DataService.IsConnected();
        }
    }
}
