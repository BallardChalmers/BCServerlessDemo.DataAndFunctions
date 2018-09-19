using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Tests
{
    public static class Utilities
    {
        public static string GetFunctionsUrl(string suffix)
        {
            return ConfigurationManager.AppSettings["functionsUrl"] + suffix;
        }
    }
}