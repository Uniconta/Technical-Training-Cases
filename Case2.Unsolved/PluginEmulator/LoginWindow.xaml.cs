using System;
using System.Threading.Tasks;
using System.Windows;
using Uniconta.Common;

namespace PluginEmulator
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        /// Here you will need to insert your own valid App id, obtained at your Uniconta partner or at Uniconta directly!
        // TODO: Change API Key
        static Guid DemoConsoleGuid = new Guid("00000000-0000-0000-0000-000000000000");

        public LoginWindow()
        {
            InitializeComponent();
            this.loginCtrl.loginButton.Click += loginButton_Click;
            this.loginCtrl.CancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {

            if (DemoConsoleGuid == Guid.Empty)
            {
                MessageBox.Show("You need to have your own valid App id, obtained at your Uniconta partner or at Uniconta directly." + Environment.NewLine + "Please insert Guid in PluginEmulator\\LoginWindow.xaml.cs file at line no 14.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string password = loginCtrl.Password;
            string userName = loginCtrl.UserName;

            ErrorCodes errorCode = await SetLogin(userName, password);

            switch (errorCode)
            {
                case ErrorCodes.Succes:
                    Uniconta.ClientTools.Localization.SetDefault((Language)DemoInitializer.CurrentSession.User._Language);
                    this.DialogResult = true;
                    break;

                case ErrorCodes.UserOrPasswordIsWrong:
                    MessageBox.Show("Please Check Your Credentials & Try again !!!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;

                default:
                    MessageBox.Show(string.Format("{0} : {1}", "Unable to login", errorCode.ToString()), "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
            }
        }

        private async Task<ErrorCodes> SetLogin(string username, string password)
        {
            if (!ValidateUserCredentials(username, password))
                return ErrorCodes.NoSucces;

            try
            {
                var ses = DemoInitializer.GetSession();
                return await ses.LoginAsync(username, password, Uniconta.Common.User.LoginType.API, DemoConsoleGuid, Uniconta.ClientTools.Localization.InititalLanguageCode);
            }
            catch(Exception ex)
            {
                MessageBox.Show("System Exception. Application Will Close.", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return ErrorCodes.Exception;
            }
        }

        private bool ValidateUserCredentials(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                return true;

            return false;
        }
    }
}
