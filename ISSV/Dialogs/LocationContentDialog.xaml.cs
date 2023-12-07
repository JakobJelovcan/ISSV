﻿using ISSV.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Dialogs
{
    public sealed partial class LocationContentDialog : ContentDialog
    {
        public LocationContentDialog(Location location)
        {
            this.InitializeComponent();
            this.Location = location;
            if (Location != null)
            {
                LocationName = Location.Name;
                Address = Location.Address.Name;
                PhoneNumber = Location.PhoneNumber;
                Email = Location.Email;
                Active = Location.Active;
                ApplyToChildren = false;
                Title = "Edit location";
            }
            else
            {
                Title = "Create location";
                Active = true;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private Location Location { get; set; }

        public string LocationName
        {
            get { return (string)GetValue(LocationNameProperty); }
            set { SetValue(LocationNameProperty, value); }
        }
        public static readonly DependencyProperty LocationNameProperty =
            DependencyProperty.Register("LocationName", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }
        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(LocationContentDialog), new PropertyMetadata(string.Empty));

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register("Active", typeof(bool), typeof(LocationContentDialog), new PropertyMetadata(false));

        public bool ApplyToChildren
        {
            get { return (bool)GetValue(ApplyToChildrenProperty); }
            set { SetValue(ApplyToChildrenProperty, value); }
        }
        public static readonly DependencyProperty ApplyToChildrenProperty =
            DependencyProperty.Register("ApplyToChildren", typeof(bool), typeof(LocationContentDialog), new PropertyMetadata(false));
    }
}
