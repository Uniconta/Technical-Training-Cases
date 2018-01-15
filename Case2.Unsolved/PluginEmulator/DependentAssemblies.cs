using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginEmulator
{
    public class DependentAssemblies
    {
        public static List<string> DependentDlls;

        public static void AddDependentDllName(string[] dllNames)
        {
            if (DependentDlls == null)
            {
                DependentDlls = new List<string>();
            }

            foreach (var dll in dllNames)
            {
                if (!DependentDlls.Contains(dll))
                {
                    DependentDlls.Add(dll);
                }
            }
        }
    }
}
