using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniconta.API.Plugin;

namespace LoadPluginEmulator
{
    public class Plugin
    {
        class PluginTypes
        {
            internal Type type;
            internal string ClassName;
        }

        List<PluginTypes> plugins;
        static Plugin loadedPlugin;
        public static Plugin LoadAssembly(string assembly)
        {
            try
            {
                if (loadedPlugin != null)
                    return loadedPlugin;

                Assembly ptrAssembly;
                if (!File.Exists(assembly))
                {
                    var errorMsg = string.Format("FileNotFound", assembly);
                    MessageBox.Show(errorMsg, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return null;
                }
                ptrAssembly = Assembly.LoadFile(assembly);

                var plugins = new List<PluginTypes>();
                if (ptrAssembly != null)
                {
                    var types = GetLoadableTypes(ptrAssembly);
                    foreach (Type item in types)
                    {
                        if (item.IsClass && item.GetInterfaces().Contains(typeof(IPluginBase)))
                            plugins.Add(new PluginTypes() { type = item, ClassName = Uniconta.Common.Utility.Util.ClassName(item) });
                    }
                }

                if (plugins.Count > 0)
                {
                    var thisPlugin = new Plugin() { plugins = plugins };
                    loadedPlugin = thisPlugin;
                    return thisPlugin;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception");
            }
            return null;
        }

        public static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public Type GetTypeFromName(string ClassName)
        {
            if (ClassName == null)
                return plugins.First().type;

            foreach (var p in plugins)
                if (string.Compare(p.ClassName, ClassName, true) == 0)
                    return p.type;
            return null;
        }

    }
}
