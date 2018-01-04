using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace PluginEmulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Uniconta.ClientTools.Localization.SetLocalizationStrings(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            Uniconta.WindowsAPI.Startup.OnLoad();
            DemoInitializer.InitUniconta();

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dllInfo = args.Name.Split(',');
            var dllName = dllInfo[0].ToString() + ".dll";

            var dependentDlls = DependentAssemblies.DependentDlls;
            if (dependentDlls == null || !dependentDlls.Contains(dllName)) return null;

            try
            {
                //for assembly resolving.. Assembly resolver  will look dependent dll on below defined path 
                var PluginPath = @"C:\Uniconta\PluginPath\";
                var assemblyPath = PluginPath + dllName;
                using (var stream = File.OpenRead(assemblyPath))
                {
                    byte[] assemblydata = new byte[stream.Length];
                    stream.Read(assemblydata, 0, assemblydata.Length);
                    return Assembly.Load(assemblydata);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Uniconta.ClientTools.Localization.lookup("Exception"));
                return null;
            }

        }

    }
}
