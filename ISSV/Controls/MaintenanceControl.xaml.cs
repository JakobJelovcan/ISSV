using ISSV.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ISSV.Controls
{
    public sealed partial class MaintenanceControl : UserControl
    {
        public MaintenanceControl()
        {
            this.InitializeComponent();
        }

        public Maintenance Maintenance
        {
            get { return (Maintenance)GetValue(MaintenanceProperty); }
            set { SetValue(MaintenanceProperty, value); }
        }
        public static readonly DependencyProperty MaintenanceProperty =
            DependencyProperty.Register("Maintenance", typeof(Maintenance), typeof(MaintenanceControl), new PropertyMetadata(null));

        private void MaintenanceControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext is Maintenance maintenance) { Maintenance = maintenance; }
        }
    }
}
