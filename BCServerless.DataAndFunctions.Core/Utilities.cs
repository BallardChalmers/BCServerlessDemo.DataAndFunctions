using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core
{
    public class Utilities
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                if (path.Contains("BCServerlessDemo.DataAndFunctions.Functions"))
                {
                    // Target parent dir as dll copied to bin folder for Functions
                    return Path.GetDirectoryName(path) + "/..";
                }
                return Path.GetDirectoryName(path);
            }
        }
    }
}
