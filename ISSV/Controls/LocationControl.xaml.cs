using ISSV.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ISSV.Controls
{
    public sealed partial class LocationControl : UserControl
    {
        public LocationControl()
        {
            this.InitializeComponent();
        }

        public Location Location
        {
            get { return (Location)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(Location), typeof(LocationControl), new PropertyMetadata(null));

        private void LocationControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext is Location location) { Location = location; }
        }
    }
}
