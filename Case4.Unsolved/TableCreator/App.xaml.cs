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
        }
    }
}
