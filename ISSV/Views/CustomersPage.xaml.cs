using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
using ISSV.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class CustomersPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Customer> Source { get; } = new ObservableCollection<Customer>();

        public CustomersPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var customers = DataService.Customers.Include(c => c.Locations).ThenInclude(l => l.Devices).ThenInclude(d => d.Maintenances);
            foreach (var customer in customers)
            {
                Source.Add(customer);
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

        private async void AddCustomerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CustomerContentDialog customerDialog = new CustomerContentDialog(null);
            await customerDialog.ShowAsync();
        }

        private async void EditMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (sender is MenuFlyoutItem flyoutItem && flyoutItem.DataContext is Customer customer)
            {
                CustomerContentDialog customerDialog = new CustomerContentDialog(customer);
                await customerDialog.ShowAsync();
            }
        }

        private void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void CustomerGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Customer customer)
            {
                NavigationService.Navigate<CustomerPage>(customer);
            }
        }
    }
}
