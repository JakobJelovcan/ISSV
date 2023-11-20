using ISSV.Core.Models;
using ISSV.Core.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Dialogs
{
    public sealed partial class CustomerContentDialog : ContentDialog
    {
        public CustomerContentDialog(Customer customer)
        {
            this.InitializeComponent();
            this.Customer = customer;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Customer != null)
            {
                if (await DataService.UpdateCustomer(Customer, CustomerName, PhoneNumber, Email, Address, Active))
                {
                    Customer.Name = CustomerName;
                    Customer.PhoneNumber = PhoneNumber;
                    Customer.Email = Email;
                    Customer.Address = Address;
                    Customer.Active = Active;
                }
            }
            else
            {
                Customer = await DataService.InsertCustomer(CustomerName, PhoneNumber, Email, Address, Active);
            }
        }

        private void ContentDialog_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        public Customer Customer { get; private set; }

        public string CustomerName
        {
            get { return (string)GetValue(CustomerNameProperty); }
            set { SetValue(CustomerNameProperty, value); }
        }
        public static readonly DependencyProperty CustomerNameProperty =
            DependencyProperty.Register("CustomerName", typeof(string), typeof(CustomerContentDialog), new PropertyMetadata(string.Empty));

        public string PhoneNumber
        {
            get { return (string)GetValue(PhoneNumberProperty); }
            set { SetValue(PhoneNumberProperty, value); }
        }
        public static readonly DependencyProperty PhoneNumberProperty =
            DependencyProperty.Register("PhoneNumber", typeof(string), typeof(CustomerContentDialog), new PropertyMetadata(string.Empty));

        public string Email
        {
            get { return (string)GetValue(EmailProperty); }
            set { SetValue(EmailProperty, value); }
        }
        public static readonly DependencyProperty EmailProperty =
            DependencyProperty.Register("Email", typeof(string), typeof(CustomerContentDialog), new PropertyMetadata(string.Empty));

        public string Address
        {
            get { return (string)GetValue(AddressProperty); }
            set { SetValue(AddressProperty, value); }
        }
        public static readonly DependencyProperty AddressProperty =
            DependencyProperty.Register("Address", typeof(string), typeof(CustomerContentDialog), new PropertyMetadata(string.Empty));

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register("Active", typeof(bool), typeof(CustomerContentDialog), new PropertyMetadata(false));
    }
}
