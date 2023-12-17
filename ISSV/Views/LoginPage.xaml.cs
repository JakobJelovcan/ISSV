using ISSV.Core.Helpers;
using ISSV.Services;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ISSV.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            if (!await IdentityService.LogInAsync(UserName, Password))
            {
                LoginFailed = true;
                Password = string.Empty;
            }
            else
            {
                LoginFailed = false;
            }
            progressRing.Visibility = Visibility.Collapsed;
        }

        public bool LoginFailed
        {
            get { return (bool)GetValue(LoginFailedProperty); }
            set { SetValue(LoginFailedProperty, value); }
        }
        public static readonly DependencyProperty LoginFailedProperty =
            DependencyProperty.Register("LoginFailed", typeof(bool), typeof(LoginPage), new PropertyMetadata(false));

        public string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }
        public static readonly DependencyProperty UserNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(LoginPage), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(LoginPage), new PropertyMetadata(string.Empty));

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;
    }
}
