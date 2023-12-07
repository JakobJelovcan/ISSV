using ISSV.Core.Models;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Dialogs
{
    public sealed partial class DeviceContentDialog : ContentDialog
    {
        public DeviceContentDialog(Device device)
        {
            this.InitializeComponent();
            this.Device = device;
            if (Device != null)
            {
                DeviceType = device.DeviceType;
                SerialNumber = device.SerialNumber;
                Active = device.Active;
                MaintenanceFrequency = device.MaintenanceFrequency;
                WarrantyPeriod = device.WarrantyPeriod;
                InstallationDate = device.InstallationDate;
                Title = "Edit device";
            }
            else
            {
                Title = "Create device";
                Active = true;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private Device Device { get; set; }

        public string DeviceType
        {
            get { return (string)GetValue(DeviceTypeProperty); }
            set { SetValue(DeviceTypeProperty, value); }
        }
        public static readonly DependencyProperty DeviceTypeProperty =
            DependencyProperty.Register("DeviceType", typeof(string), typeof(DeviceContentDialog), new PropertyMetadata(string.Empty));

        public string SerialNumber
        {
            get { return (string)GetValue(SerialNumberProperty); }
            set { SetValue(SerialNumberProperty, value); }
        }
        public static readonly DependencyProperty SerialNumberProperty =
            DependencyProperty.Register("SerialNumber", typeof(string), typeof(DeviceContentDialog), new PropertyMetadata(string.Empty));

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register("Active", typeof(bool), typeof(DeviceContentDialog), new PropertyMetadata(false));

        public int MaintenanceFrequency
        {
            get { return (int)GetValue(MaintenanceFrequencyProperty); }
            set { SetValue(MaintenanceFrequencyProperty, value); }
        }
        public static readonly DependencyProperty MaintenanceFrequencyProperty =
            DependencyProperty.Register("MaintenanceFrequency", typeof(int), typeof(DeviceContentDialog), new PropertyMetadata(0));

        public int WarrantyPeriod
        {
            get { return (int)GetValue(WarrantyPeriodProperty); }
            set { SetValue(WarrantyPeriodProperty, value); }
        }
        public static readonly DependencyProperty WarrantyPeriodProperty =
            DependencyProperty.Register("WarrantyPeriod", typeof(int), typeof(DeviceContentDialog), new PropertyMetadata(0));

        public DateTimeOffset InstallationDate
        {
            get { return (DateTimeOffset)GetValue(InstallationDateProperty); }
            set { SetValue(InstallationDateProperty, value); }
        }
        public static readonly DependencyProperty InstallationDateProperty =
            DependencyProperty.Register("InstallationDate", typeof(DateTimeOffset), typeof(DeviceContentDialog), new PropertyMetadata(DateTimeOffset.Now));
    }
}
