using System.Windows;
using Uniconta.Common;
using ZendoImporter.Core.Managers;

namespace ZendoImporter.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var errorCode = await UnicontaAPIManager.Login(this.TB_Username.Text, this.TB_Password.Password);
            if (errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Login Failed" + errorCode.ToString());
                return;
            }
            await UnicontaAPIManager.InitializeCompanies();

            // Open MainWindow
            var window = new MainWindow();
            window.Show();
            window.Activate();
        }
    }
}
