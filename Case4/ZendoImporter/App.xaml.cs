using System.Windows;
using Uniconta.Common;
using ZendoImporter.Core.Managers;

namespace ZendoImporter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            UnicontaAPIManager.Initialize();

            // TODO: Change username and password
            var errorCode = UnicontaAPIManager.Login("USERNAME", "PASSWORD").Result;
            if (errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Login Failed" + errorCode.ToString());
                System.Windows.Application.Current.Shutdown();
                return;
            }

            UnicontaAPIManager.InitializeCompanies().Wait();
        }
    }
}
