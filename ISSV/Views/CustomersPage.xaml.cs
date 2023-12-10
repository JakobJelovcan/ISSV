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
            var customers = DataService.Customers.Include(c => c.Locations).ThenInclude(l => l.Devices).ThenInclude(d => d.Maintenances).OrderBy(c => c.Name);
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
            var dialog = new CustomerContentDialog(null);
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Source.Add(dialog.Customer);
            }
        }

        private void CustomerGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Customer customer)
            {
                NavigationService.Navigate<CustomerPage>(customer);
            }
        }

        private async void EditMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).DataContext is Customer customer)
            {
                await new CustomerContentDialog(customer).ShowAsync();
            }
        }

        private async void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).DataContext is Customer customer)
            {
                var res = await new DeleteDialog().ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    customer.Delete();
                    Source.Remove(customer);
                    await DataService.SaveChangesAsync();
                }
            }
        }
    }
}
