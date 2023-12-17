using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
using ISSV.Helpers;
using ISSV.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class CustomerPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Location> Source { get; } = new ObservableCollection<Location>();

        public Customer Customer
        {
            get => customer;
            set => Set(ref customer, value);
        }
        private Customer customer;

        public CustomerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Customer customer)
            {
                Customer = DataService.Customers.Where(c => c.Equals(customer)).FirstOrDefault();
                var locations = DataService.Locations.Include(l => l.Address).Include(l => l.Devices).ThenInclude(d => d.Maintenances).Where(l => l.Customer.Equals(customer)).OrderBy(l => l.Name);
                foreach (var location in locations)
                {
                    Source.Add(location);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void LocationGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Location location)
            {
                NavigationService.Navigate<LocationPage>(location);
            }
        }

        private async void EditCustomerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new CustomerContentDialog(Customer).ShowAsync();
        }

        private async void DeleteCustomerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var res = await new DeleteDialog().ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Customer.Delete();
                await DataService.SaveChangesAsync();
                NavigationService.GoBack();
            }
        }

        private async void AddLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new LocationContentDialog(Customer, null);
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Source.Add(dialog.Location);
            }
        }

        private async void EditLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new LocationContentDialog(Customer, (sender as MenuFlyoutItem).DataContext as Location).ShowAsync();
        }

        private async void DeleteLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).DataContext is Location location)
            {
                var res = await new DeleteDialog().ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    location.Delete();
                    Source.Remove(location);
                    await DataService.SaveChangesAsync();
                }
            }
        }
    }
}
