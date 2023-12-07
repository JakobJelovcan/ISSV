using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
using ISSV.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Composition;
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
                var locations = DataService.Locations.Include(l => l.Address).Include(l => l.Devices).ThenInclude(d => d.Maintenances).Where(l => l.Customer.Equals(customer));
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

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void LocationGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Location location)
            {
                NavigationService.Navigate<LocationPage>(location);
            }
        }

        private async void EditCustomerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CustomerContentDialog dialog = new CustomerContentDialog(Customer);
            await dialog.ShowAsync();
        }

        private void DeleteCustomerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Delete dialog
        }

        private async void AddLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LocationContentDialog dialog = new LocationContentDialog(null);
            await dialog.ShowAsync();
        }

        private async void EditMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            LocationContentDialog dialog = new LocationContentDialog((sender as MenuFlyoutItem).DataContext as Location);
            await dialog.ShowAsync();
        }

        private void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Delete dialog
        }
    }
}
