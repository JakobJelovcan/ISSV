using ISSV.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Tools;

namespace ISSV.Views
{
    public sealed partial class HomePage : Page, INotifyPropertyChanged
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public ObservableCollection<DataPoint> DeviceStatus { get; } = new ObservableCollection<DataPoint>();

        public ObservableCollection<DataPoint> PerformedMaintenances { get; } = new ObservableCollection<DataPoint>();

        public ObservableCollection<DataPoint> DeviceAge { get; } = new ObservableCollection<DataPoint>();

        public ObservableCollection<DataPoint> RegularMaintenances { get; } = new ObservableCollection<DataPoint>();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DeviceStatus.Clear();
            var deviceStatus = DataService.Devices.AsEnumerable().GroupBy(d => new { d.RequiresMaintenance, d.Active });
            DeviceStatus.Add(new DataPoint("Requires maintenance", deviceStatus.Where(s => s.Key.RequiresMaintenance && s.Key.Active)?.FirstOrDefault()?.Count() ?? 0));
            DeviceStatus.Add(new DataPoint("Doesn't require maintenance", deviceStatus.Where(s => !s.Key.RequiresMaintenance && s.Key.Active)?.FirstOrDefault()?.Count() ?? 0));
            DeviceStatus.Add(new DataPoint("Inactive", deviceStatus.Where(s => !s.Key.Active)?.FirstOrDefault()?.Count() ?? 0));

            PerformedMaintenances.Clear();
            DataService.Maintenances.AsEnumerable().GroupBy(m => new { m.Date.Month, m.Date.Year }).OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month).ForEach(g => PerformedMaintenances.Add(new DataPoint($"{g.Key.Month}/{g.Key.Year}", g.Count())));

            DeviceAge.Clear();
            DataService.Devices.AsEnumerable().Where(d => d.Active).GroupBy(d => DateTimeOffset.Now.Year - d.InstallationDate.Year).OrderBy(g => g.Key).ForEach(g => DeviceAge.Add(new DataPoint($"{g.Key}", g.Count())));

            RegularMaintenances.Clear();
            RegularMaintenances.Add(new DataPoint("Yes", DataService.Maintenances.Where(m => m.RegularMaintenance).Count()));
            RegularMaintenances.Add(new DataPoint("No", DataService.Maintenances.Where(m => !m.RegularMaintenance).Count()));
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

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DataPoint : INotifyPropertyChanged
    {
        public DataPoint(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        private string name;

        public int Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }
        private int value;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
