using System.Windows;
using TableCreator.Core.Managers;
using Uniconta.Common;

namespace TableCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            UnicontaAPIManager.Initialize();

            // TODO: Enter username and password
            var errorCode = UnicontaAPIManager.Login("USERNAME", "PASSWORD").Result;
            if(errorCode != ErrorCodes.Succes)
            {
                MessageBox.Show("ERROR: Login Failed" + errorCode.ToString());
                System.Windows.Application.Current.Shutdown();
                return;
            }

            UnicontaAPIManager.InitializeCompanies().Wait();
        }
    }
}
