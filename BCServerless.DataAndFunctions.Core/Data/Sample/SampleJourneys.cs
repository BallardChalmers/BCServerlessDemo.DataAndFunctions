using EnumsNET;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleJourneys
    {
        public static List<Journey> All => ImportJourneysFromFile();

        public static List<Journey> ImportJourneysFromFile()
        {
            List<Journey> journeys;
            try
            {
                string vehiclesSamplePath = Utilities.AssemblyDirectory + "/../JourneyData.csv";

                System.IO.TextReader reader = File.OpenText(vehiclesSamplePath);

                var csv = new CsvReader(reader);
                csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-GB");
                var records = csv.GetRecords<Journey>();
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                journeys = records.ToList<Journey>();
                return journeys;
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to retrieve journeys");
            }
        }
    }
}
