using BCServerlessDemo.DataAndFunctions.Core.Data.Base;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleVehicles
    {
        public static List<Vehicle> All => ImportJourneysFromFile();

        public static List<Vehicle> ImportJourneysFromFile()
        {
            string vehiclesSamplePath = AssemblyDirectory + "/../Automobile_data-clean.csv";
            
            System.IO.TextReader reader = File.OpenText(vehiclesSamplePath);

            var csv = new CsvReader(reader);
            
            var records = csv.GetRecords<Vehicle>();

            return records.ToList<Vehicle>();
        }

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
