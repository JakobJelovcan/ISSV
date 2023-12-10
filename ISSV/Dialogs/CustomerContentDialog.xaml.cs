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
            if (Customer != null)
            {
                CustomerName = Customer.Name;
                PhoneNumber = Customer.PhoneNumber;
                Email = Customer.Email;
                Active = Customer.Active;
                Title = "Edit customer";
            }
            else
            {
                Title = "Create customer";
                Active = true;
            }
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (Customer is null)
            {
                Customer = new Customer(Name, PhoneNumber, Email, Active);
                DataService.Customers.Add(Customer);
            }
            else
            {
                Customer.Update(Name, PhoneNumber, Email, Active);
            }
            await DataService.SaveChangesAsync();
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

        public bool Active
        {
            get { return (bool)GetValue(ActiveProperty); }
            set { SetValue(ActiveProperty, value); }
        }
        public static readonly DependencyProperty ActiveProperty =
            DependencyProperty.Register("Active", typeof(bool), typeof(CustomerContentDialog), new PropertyMetadata(false));
    }
}
