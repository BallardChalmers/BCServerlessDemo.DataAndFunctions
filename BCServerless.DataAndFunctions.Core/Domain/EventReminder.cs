using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class EventReminder : BaseModel
        {
            public static string eventId = "3254e288-a839-4de7-89c7-2d9a2362b3c4";

            public string description { get; set; }
            public DateTime date { get; set; }
            public bool showOnDashboard { get; set; }
        }
    }
