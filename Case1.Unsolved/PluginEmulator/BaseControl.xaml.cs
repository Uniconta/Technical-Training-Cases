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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Uniconta.API.Plugin;

namespace PluginEmulator
{
    public partial class BaseControl : UserControl
    {
        IContentPluginBase pluginContent;
        public BaseControl(IContentPluginBase pluginClass)
        {
            InitializeComponent();
            pluginContent = pluginClass;
            pluginClass.SetContent(this);

            this.Unloaded += BaseControl_Unloaded;
        }

        private void BaseControl_Unloaded(object sender, RoutedEventArgs e)
        {
            pluginContent.OnPageClose();
            this.Content = null;
        }
    }
}
