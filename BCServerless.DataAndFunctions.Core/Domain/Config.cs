using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Config : BaseModel
    {
        public static string ConfigId = "22c4bf69-0582-4052-9ed0-0c9cbd0341f3";
        
        public bool BaseDataSetup { get; set; }
        public int JourneyCounter { get; set; }
        public bool RecertificationsTriggered { get; set; }
        public bool JourneyApprovalsEnabled { get; set; }

        public static DateTime GetCertificationDate(DateTime currentDate)
        {
            var currentYearDate = new DateTime(currentDate.Year, 3, 31);
            return currentYearDate <= currentDate ? currentYearDate.AddYears(1) : currentYearDate; 
        }
    }
}
