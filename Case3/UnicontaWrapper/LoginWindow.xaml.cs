using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TableCreator.Core.Managers;
using Uniconta.Common;

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
            var loginErrorCode = await UnicontaAPIManager.Login(this.TB_Username.Text, this.PB_Password.Password);
            if (loginErrorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Login Failed" + loginErrorCode.ToString());
                return;
            }

            await UnicontaAPIManager.InitializeCompanies();

            var window = new MainWindow();
            window.Show();
            window.Activate();
        }
    }
}
