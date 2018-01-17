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
        }
    }
}
