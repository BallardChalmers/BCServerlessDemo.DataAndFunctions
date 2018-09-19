using EnumsNET;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleJourneys
    {
        public static List<Journey> All => new List<Journey>()
        {
            Journey1, Journey2
        };

        public static Journey Journey1 => new Journey()
        {
            id = "1985651c-f63f-4bd0-9ebf-888f46376c66",
            name = "Journey 1"
        };

        public static Journey Journey2 => new Journey()
        {
            id = "1985651c-f63f-4bd0-9ebf-888f46376c66",
            name = "Journey 2"
        };
    }
}
