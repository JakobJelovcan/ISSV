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
    public sealed partial class LocationPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Device> Source { get; } = new ObservableCollection<Device>();

        public Location Location
        {
            get => location;
            set => Set(ref location, value);
        }
        private Location location;

        public LocationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Location location)
            {
                Location = DataService.Locations.Where(l => l.Equals(location)).FirstOrDefault();
                var devices = DataService.Devices.Include(d => d.Location).Include(d => d.Maintenances).Where(d => d.Location.Equals(location)).OrderBy(d => d.DeviceType);
                foreach (var device in devices)
                {
                    Source.Add(device);
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

        private void DeviceGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Device device)
            {
                NavigationService.Navigate<DevicePage>(device);
            }
        }

        private async void EditLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new LocationContentDialog(Location.Customer, Location).ShowAsync();
        }

        private async void DeleteLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var res = await new DeleteDialog().ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Location.Delete();
                await DataService.SaveChangesAsync();
                NavigationService.GoBack();
            }
        }

        private async void AddDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new DeviceContentDialog(Location, null);
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Source.Add(dialog.Device);
            }
        }

        private async void EditDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new DeviceContentDialog(Location, (sender as MenuFlyoutItem).DataContext as Device).ShowAsync();
        }

        private async void DeleteDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).DataContext is Device device)
            {
                var res = await new DeleteDialog().ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    device.Delete();
                    Source.Remove(device);
                    await DataService.SaveChangesAsync();
                }
            }
        }
    }
}
