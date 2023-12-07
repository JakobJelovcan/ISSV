﻿using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Dialogs;
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
                var maintenances = DataService.Maintenances.Where(m => m.Device.Equals(device));
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

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private async void EditMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MaintenanceContentDialog dialog = new MaintenanceContentDialog((sender as MenuFlyoutItem).DataContext as Maintenance);
            await dialog.ShowAsync();
        }

        private void DeleteMenuFlyoutItem_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Delete dialog
        }

        private async void EditDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            DeviceContentDialog dialog = new DeviceContentDialog(Device);
            await dialog.ShowAsync();
        }

        private void DeleteDeviceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            //TODO: Delete dialog
        }

        private async void AddMaintenanceButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MaintenanceContentDialog dialog = new MaintenanceContentDialog(null);
            await dialog.ShowAsync();
        }
    }
}