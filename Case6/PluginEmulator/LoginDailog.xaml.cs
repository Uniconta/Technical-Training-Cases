using System.Windows.Controls;

namespace PluginEmulator
{
    /// <summary>
    /// Interaction logic for LoginDailog.xaml
    /// </summary>
    public partial class LoginDailog : UserControl
    {
        public LoginDailog()
        {
            InitializeComponent();
        }

        public string UserName
        {
            get { return txtUserName.Text; }
        }

        public string Password
        {
            get { return txtPassword.Password; }
        }
    }
}
