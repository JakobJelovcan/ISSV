using ISSV.Core.Models;
using ISSV.Core.Services;
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
        ObservableCollection<Location> Source = new ObservableCollection<Location>();

        public LocationPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Customer customer)
            {
                var locations = DataService.Locations.Include(l => l.Address).Include(l => l.Devices).Where(l => l.Customer == customer);
                foreach (var location in locations)
                {
                    Source.Add(location);
                }
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

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

        }

        private void LocationGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
