using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ISSV.Core.Models;
using ISSV.Core.Services;
using ISSV.Helpers;
using ISSV.Services;

using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Navigation;

namespace ISSV.Views
{
    public sealed partial class MapPage : Page, INotifyPropertyChanged
    {
        // TODO: Set your preferred default zoom level
        private const double DefaultZoomLevel = 17;

        private readonly LocationService _locationService;

        // TODO: Set your preferred default location if a geolock can't be found.
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

        public Geopoint Center
        {
            get { return _center; }
            set { Set(ref _center, value); }
        }

        public ObservableCollection<SampleCompany> Source { get; } = new ObservableCollection<SampleCompany>();

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
            Center = new Geopoint(_defaultPosition);
            ZoomLevel = DefaultZoomLevel;
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitializeAsync();
            var data = await SqlServerDataService.AllCompanies();
            foreach (var company in data)
            {
                Source.Add(company);
            }

            await AddLocations();
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

            if (mapControl != null)
            {
                // TODO: Set your map service token. If you don't have one, request from https://www.bingmapsportal.com/
                // mapControl.MapServiceToken = string.Empty;
                AddMapIcon(Center, "Map_YourLocation".GetLocalized());
            }
        }

        private async Task AddLocations()
        {
            var tasks = await Task.WhenAll(Source.Select(c => GetLocation(c)));
            var locations = tasks.Where(e => e != null).ToList();
            var layer = new MapElementsLayer
            {
                ZIndex = 1,
                MapElements = locations
            };

            mapControl.Layers.Add(layer);
        }

        private async Task<MapElement> GetLocation(SampleCompany company)
        {
            var address = $"{company.Address}, {company.City}, {company.Country}";

            var location = await MapLocationService.GetMapLocationAsync(address);
            if (location != null)
            {
                return new MapIcon
                {
                    Location = location.Point,
                    NormalizedAnchorPoint = new Point(0.5, 1),
                    ZIndex = 0,
                    Title = location.DisplayName
                };
            }
            return null;
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

        private void AddMapIcon(Geopoint position, string title)
        {
            MapIcon mapIcon = new MapIcon()
            {
                Location = position,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = title,
                Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/map.png")),
                ZIndex = 0
            };
            mapControl.MapElements.Add(mapIcon);
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
            MapSplitView.IsPaneOpen ^= true;
        }
    }
}
