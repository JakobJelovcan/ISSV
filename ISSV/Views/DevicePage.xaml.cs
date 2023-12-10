using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
using ISSV.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class DevicePage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Maintenance> Source { get; } = new ObservableCollection<Maintenance>();

        public Device Device
        {
            get => device;
            set => Set(ref device, value);
        }
        private Device device;

        public DevicePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Device device)
            {
                Device = DataService.Devices.Where(d => d.Equals(device)).FirstOrDefault();
                var maintenances = DataService.Maintenances.Where(m => m.Device.Equals(device)).OrderByDescending(m => m.Date);
                foreach (var maintenance in maintenances)
                {
                    Source.Add(maintenance);
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

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Maintenance maintenance)
            {
                NavigationService.Navigate<MaintenancePage>(maintenance);
            }
        }

        private async void EditMaintenanceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new MaintenanceContentDialog(Device, (sender as MenuFlyoutItem).DataContext as Maintenance).ShowAsync();
        }

        private async void DeleteMaintenanceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if ((sender as MenuFlyoutItem).DataContext is Maintenance maintenance)
            {
                var res = await new DeleteDialog().ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    maintenance.Delete();
                    Source.Remove(maintenance);
                    await DataService.SaveChangesAsync();
                }
            }
        }

        private async void EditDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new DeviceContentDialog(Device.Location, Device).ShowAsync();
        }

        private async void DeleteDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var res = await new DeleteDialog().ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Device.Delete();
                await DataService.SaveChangesAsync();
                NavigationService.GoBack();
            }
        }

        private async void AddMaintenanceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var dialog = new MaintenanceContentDialog(Device, null);
            var res = await dialog.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                Source.Add(dialog.Maintenance);
            }
        }
    }
}
