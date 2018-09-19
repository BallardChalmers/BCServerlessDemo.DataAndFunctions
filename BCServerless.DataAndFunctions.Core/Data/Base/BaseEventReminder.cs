using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Base
{
    public static class BaseEventReminder
    {
        public static EventReminder EventReminder => new EventReminder
        {
            id = EventReminder.eventId,
            name = "Placeholder event reminder",
            description = "Placeholder to be updated in the admin UI",
            date = DateTime.SpecifyKind(new DateTime(2018, 12, 25), DateTimeKind.Utc),
            showOnDashboard = false
        };      
    }
}
