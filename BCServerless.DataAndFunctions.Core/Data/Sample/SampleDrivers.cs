using EnumsNET;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleDrivers
    {
        public static List<Driver> All
        {
            get
            {
                var list = ImportDriversFromFile();
                return list;
            }
        }
        

        public static List<Driver> ImportDriversFromFile()
        {
            List<Driver> drivers;
            try
            {
                string vehiclesSamplePath = Utilities.AssemblyDirectory + "/../street-hail-livery-shl-drivers-active.csv";

                System.IO.TextReader reader = File.OpenText(vehiclesSamplePath);

                var csv = new CsvReader(reader);
                csv.Configuration.CultureInfo = CultureInfo.GetCultureInfo("en-GB");
                var records = csv.GetRecords<Driver>();
                csv.Configuration.MissingFieldFound = null;
                csv.Configuration.HeaderValidated = null;
                drivers = records.ToList();

                return drivers;
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to retrieve journeys");
            }
        }
        /*
        public static Driver Driver1 => new Driver()
        {
            id = "7051831e-3ec9-4615-bb6f-a8b0bf709b7e",
            GivenName = "John",
            Surname = "Surtees",
            Description = "John Surtees, CBE (11 February 1934 – 10 March 2017) was an English Grand Prix motorcycle road racer and Formula One driver. He was a four-time 500cc motorcycle World Champion – winning that title in 1956, 1958, 1959 and 1960 – the Formula One World Champion in 1964, and remains the only person to have won World Championships on both two and four wheels. He founded the Surtees Racing Organisation team that competed as a constructor in Formula One, Formula 2 and Formula 5000 from 1970 to 1978. He was also the ambassador of the Racing Steps Foundation."
        };
        
        public static Driver Driver2 => new Driver()
        {
            id = "e17ff2bc-df8f-40ce-8979-d017c9bf28b8",
            GivenName = "Damon",
            Surname = "Hill",
            Description = "Damon Graham Devereux Hill, OBE (born 17 September 1960) is a British former racing driver. He is the son of Graham Hill, and, along with Nico Rosberg, one of only two sons of a Formula One world champion to also win the title. He started racing on motorbikes in 1981, and after minor success moved on to single-seater racing cars. Despite progressing steadily up the ranks to the International Formula 3000 championship by 1989, and often being competitive, he never won a race at that level."
        };
        */
    }
}
