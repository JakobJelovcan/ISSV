using ISSV.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ISSV.Controls
{
    public sealed partial class DeviceControl : UserControl
    {
        public DeviceControl()
        {
            this.InitializeComponent();
        }

        public Device Device
        {
            get { return (Device)GetValue(DeviceProperty); }
            set { SetValue(DeviceProperty, value); }
        }
        public static readonly DependencyProperty DeviceProperty =
            DependencyProperty.Register("Device", typeof(Device), typeof(DeviceControl), new PropertyMetadata(null));

        private void DeviceControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext is Device device) { Device = device; }
        }
    }
}
