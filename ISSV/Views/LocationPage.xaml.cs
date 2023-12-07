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
                var devices = DataService.Devices.Include(d => d.Location).Include(d => d.Maintenances).Where(d => d.Location.Equals(location));
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
            LocationContentDialog dialog = new LocationContentDialog(Location);
            await dialog.ShowAsync();
        }

        private void DeleteLocationButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Delete dialog
        }

        private async void AddDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DeviceContentDialog dialog = new DeviceContentDialog(null);
            await dialog.ShowAsync();
        }
    }
}
