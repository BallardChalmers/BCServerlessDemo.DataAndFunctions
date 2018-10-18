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
        public static List<Vehicle> All => ImportVehiclesFromFile();

        public static List<Vehicle> ImportVehiclesFromFile()
        {
            string vehiclesSamplePath = Utilities.AssemblyDirectory + "/../Automobile_data-clean.csv";
            
            System.IO.TextReader reader = File.OpenText(vehiclesSamplePath);

            var csv = new CsvReader(reader);
            
            var records = csv.GetRecords<Vehicle>();

            return records.ToList<Vehicle>();
        }
    }
}
