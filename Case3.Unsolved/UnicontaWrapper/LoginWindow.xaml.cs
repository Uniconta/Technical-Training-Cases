using System.Windows;

namespace UnicontaWrapper
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
            var username = this.TB_Username.Text;
            var password = this.PB_Password.Password;

            // TODO: Implement the Login Method

            // TODO: Initialize Companies

            var window = new MainWindow();
            window.Show();
            window.Activate();
        }
    }
}
