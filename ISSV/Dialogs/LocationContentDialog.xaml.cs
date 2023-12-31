using CommunityToolkit.Common;
using CommunityToolkit.WinUI;
using ISSV.Core.Models;
using ISSV.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Dialogs
{
    public sealed partial class LocationContentDialog : ContentDialog
    {
        public LocationContentDialog(Customer customer, Location location)
        {
            this.InitializeComponent();
            this.Location = location;
            this.Customer = customer;
            if (Location != null)
            {
                LocationName = Location.Name;
                Address = Location.Address.Name;
                PhoneNumber = Location.PhoneNumber;
                Email = Location.Email;
                Active = Location.Active;
                Title = "LocationDialog_TitleEdit".GetLocalized();
            }
            else
            {
                Title = "LocationDialog_TitleCreate".GetLocalized();
                Active = true;
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var deferral = args.GetDeferral();
            var address = DataService.Addresses.Where(a => a.Name == Address).FirstOrDefault() ?? await CreateAddress(Address);
            if (address is null)
            {
                addressInfoBar.IsOpen = true;
                args.Cancel = true;
                return;
            }
            else
            {
                addressInfoBar.IsOpen = false;
            }

            if (Location is null)
            {
                Location = new Location(Customer, LocationName, address, PhoneNumber, Email, Active);
                Customer.AddLocation(Location);
                DataService.Locations.Add(Location);
            }
            else
            {
                Location.Update(LocationName, address, PhoneNumber, Email, Active);
            }
            await DataService.SaveChangesAsync();
            deferral.Complete();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Addresses.Clear();
            var addresses = DataService.Addresses.Where(a => a.Name.StartsWith(sender.Text));
            foreach (var address in addresses)
            {
                Addresses.Add(address.Name);
            }
            if (!addresses.Any())
            {
                Addresses.Add("No results found");
            }
        }

        private async Task<Address> CreateAddress(string address)
        {
            var res = await MapLocationFinder.FindLocationsAsync(address, null, 1);
            if (res.Status == MapLocationFinderStatus.Success && res.Locations.Any())
            {
                var position = res.Locations.FirstOrDefault().Point.Position;
                var addr = new Address(address, position.Latitude, position.Longitude);
                DataService.Addresses.Add(addr);
                return addr;
            }
            return null;
        }

        public Location Location { get; private set; }

        private Customer Customer { get; set; }

        private ObservableCollection<string> Addresses { get; } = new ObservableCollection<string>();

        public string LocationName
        {
            get { return (string)GetValue(LocationNameProperty); }
            set { SetValue(LocationNameProperty, value); }
        }
        public static readonly DependencyProperty LocationNameProperty =
            DependencyProperty.Register("LocationName", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }
        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register("Active", typeof(bool), typeof(LocationContentDialog), new PropertyMetadata(false));
    }
}
