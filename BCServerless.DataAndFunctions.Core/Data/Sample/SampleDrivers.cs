using EnumsNET;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleDrivers
    {
        public static List<Driver> Driver => new List<Driver>()
        {
            Driver1, Driver2
        };

        public static List<Driver> All
        {
            get
            {
                var list = new List<Driver>() { Driver1, Driver2, };
                return list;
            }
        }

        public static Driver Driver1 => new Driver()
        {
            id = "7051831e-3ec9-4615-bb6f-a8b0bf709b7e"
        };
        
        public static Driver Driver2 => new Driver()
        {
            id = "e17ff2bc-df8f-40ce-8979-d017c9bf28b8"
        }; 
    }
}
