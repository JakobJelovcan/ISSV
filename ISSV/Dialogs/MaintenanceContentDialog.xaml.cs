using ISSV.Core.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Dialogs
{
    public sealed partial class MaintenanceContentDialog : ContentDialog
    {
        public MaintenanceContentDialog(Maintenance maintenance)
        {
            this.InitializeComponent();
            this.maintenance = maintenance;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private Maintenance maintenance { get; set; }

        public DateTimeOffset Date
        {
            get { return (DateTimeOffset)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(DateTimeOffset), typeof(MaintenanceContentDialog), new PropertyMetadata(DateTimeOffset.Now));

        public string Reason
        {
            get { return (string)GetValue(ReasonProperty); }
            set { SetValue(ReasonProperty, value); }
        }
        public static readonly DependencyProperty ReasonProperty =
            DependencyProperty.Register("Reason", typeof(string), typeof(MaintenanceContentDialog), new PropertyMetadata(false));

        public string WorkDone
        {
            get { return (string)GetValue(WorkDoneProperty); }
            set { SetValue(WorkDoneProperty, value); }
        }
        public static readonly DependencyProperty WorkDoneProperty =
            DependencyProperty.Register("WorkDone", typeof(string), typeof(MaintenanceContentDialog), new PropertyMetadata(string.Empty));

        public string Notes
        {
            get { return (string)GetValue(NotesProperty); }
            set { SetValue(NotesProperty, value); }
        }
        public static readonly DependencyProperty NotesProperty =
            DependencyProperty.Register("Notes", typeof(string), typeof(MaintenanceContentDialog), new PropertyMetadata(string.Empty));

        public string WorkOrder
        {
            get { return (string)GetValue(WorkOrderProperty); }
            set { SetValue(WorkOrderProperty, value); }
        }
        public static readonly DependencyProperty WorkOrderProperty =
            DependencyProperty.Register("WorkOrder", typeof(string), typeof(MaintenanceContentDialog), new PropertyMetadata(string.Empty));

        public string Repairman
        {
            get { return (string)GetValue(RepairmanProperty); }
            set { SetValue(RepairmanProperty, value); }
        }
        public static readonly DependencyProperty RepairmanProperty =
            DependencyProperty.Register("Repairman", typeof(string), typeof(MaintenanceContentDialog), new PropertyMetadata(string.Empty));

        public bool RegularMaintenance
        {
            get { return (bool)GetValue(RegularMaintenanceProperty); }
            set { SetValue(RegularMaintenanceProperty, value); }
        }
        public static readonly DependencyProperty RegularMaintenanceProperty =
            DependencyProperty.Register("RegularMaintenance", typeof(bool), typeof(MaintenanceContentDialog), new PropertyMetadata(false));
    }
}
