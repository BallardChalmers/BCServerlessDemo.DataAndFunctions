using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class AdminDashboardOverview
    {
        public AdminDashboardOverview()
        {
        }

        public int driversNotExpiring { get; set; }
        public int driversTotal { get; set; }
        public int orgsNotExpiring { get; set; }
        public int orgsTotal { get; set; }

        public string orgId { get; set; }
        public string orgName { get; set; }
        public string orgLogoId { get; set; }
    }
}
