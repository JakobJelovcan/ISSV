using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class MapPage : Page, INotifyPropertyChanged
    {
        private const double DefaultZoomLevel = 17;

        private readonly LocationService _locationService;

        private readonly BasicGeoposition _defaultPosition = new BasicGeoposition()
        {
            Latitude = 47.609425,
            Longitude = -122.3417
        };

        private double _zoomLevel;

        public double ZoomLevel
        {
            get { return _zoomLevel; }
            set { Set(ref _zoomLevel, value); }
        }

        private Geopoint _center;

        private MapElementsLayer _layer;

        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        public ObservableCollection<FilterItem> Source { get; } = new ObservableCollection<FilterItem>();

        public MapColorScheme MapColorScheme
        {
            get
            {
                switch (ThemeSelectorService.Theme)
                {
                    case Windows.UI.Xaml.ElementTheme.Dark: return MapColorScheme.Dark;
                    case Windows.UI.Xaml.ElementTheme.Light: return MapColorScheme.Light;
                    default: return (Windows.UI.Xaml.Application.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Dark) ? MapColorScheme.Dark : MapColorScheme.Light;
                }
            }
        }

        public MapPage()
        {
            _locationService = new LocationService();
            _layer = new MapElementsLayer
            {
                ZIndex = 1,
            };
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitializeAsync();
            var customers = DataService.Customers.Include(c => c.Locations).ThenInclude(l => l.Address).OrderBy(c => c.Name);
            foreach (var customer in customers)
            {
                Source.Add(new FilterItem(customer));
            }

            if (mapControl != null)
            {
                mapControl.Layers.Add(_layer);
                UpdateMapIcons();
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Cleanup();
        }

        public async Task InitializeAsync()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged += LocationService_PositionChanged;

                var initializationSuccessful = await _locationService.InitializeAsync();

                if (initializationSuccessful)
                {
                    await _locationService.StartListeningAsync();
                }

                if (initializationSuccessful && _locationService.CurrentPosition != null)
                {
                    Center = _locationService.CurrentPosition.Coordinate.Point;
                }
                else
                {
                    Center = new Geopoint(_defaultPosition);
                }
            }
        }

        public void Cleanup()
        {
            if (_locationService != null)
            {
                _locationService.PositionChanged -= LocationService_PositionChanged;
                _locationService.StopListening();
            }
        }

        private void LocationService_PositionChanged(object sender, Geoposition geoposition)
        {
            if (geoposition != null)
            {
                Center = geoposition.Coordinate.Point;
            }
        }

        private void UpdateMapIcons()
        {
            var mapElements = new List<MapElement>();
            foreach (var filterItem in Source)
            {
                if (filterItem.IsChecked)
                {
                    foreach (var location in filterItem.Customer.Locations)
                    {
                        var geoPosition = new BasicGeoposition { Latitude = location.Address.Latitude, Longitude = location.Address.Longitude };
                        var geoPoint = new Geopoint(geoPosition);
                        var mapIcon = new MapIcon()
                        {
                            Location = geoPoint,
                            NormalizedAnchorPoint = new Point(0.5, 1.0),
                            Title = location.Address.Name,
                            ZIndex = 0,
                            Tag = location,
                        };
                        mapElements.Add(mapIcon);
                    }
                }
            }
            _layer.MapElements = mapElements;
        }

        private void FilterButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MapSplitView.IsPaneOpen ^= true;
        }

        private void ShowAllButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (var filterItem in Source)
            {
                filterItem.IsChecked = true;
            }
            UpdateMapIcons();
        }

        private void HideAllButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            foreach (var filterItem in Source)
            {
                filterItem.IsChecked = false;
            }
            UpdateMapIcons();
        }

        private void MapControl_MapElementClick(MapControl sender, MapElementClickEventArgs args)
        {

        }

        private void CheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateMapIcons();
        }

        private void CheckBox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UpdateMapIcons();
        }

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
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class FilterItem : INotifyPropertyChanged
    {
        public FilterItem(Customer customer)
        {
            IsChecked = true;
            Customer = customer;
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                {
                    isChecked = value;
                    RaisePropertyChanged(nameof(IsChecked));
                }
            }
        }
        private bool isChecked;

        public Customer Customer
        {
            get { return customer; }
            set
            {
                if (customer != value)
                {
                    customer = value;
                    RaisePropertyChanged(nameof(Customer));
                }
            }
        }
        private Customer customer;

        private void RaisePropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
