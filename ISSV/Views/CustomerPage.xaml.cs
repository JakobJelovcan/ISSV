using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class CustomerPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<SampleOrder> Source { get; } = new ObservableCollection<SampleOrder>();

        public CustomerPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var data = await DataService.GetContentListDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
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

        private async void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }
    }
}
