using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TableCreator.Core.Managers;
using Uniconta.Common;

namespace UnicontaWrapper
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
