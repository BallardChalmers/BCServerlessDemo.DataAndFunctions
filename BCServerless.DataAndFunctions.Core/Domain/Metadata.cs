using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Metadata : BaseModel
        {
            public const string MetadataId = "6105b2b5-98ae-4060-a991-f5b68d1e51e3";

            public Metadata()
            {
                checklistItems = new List<LookupString>();
                riskHazardTypes = new List<LookupString>();
                riskPeople = new List<LookupString>();
                ethnicities = new List<LookupString>();
                learnerRatingQuestions = new List<LookupString>();
            }

            public IList<LookupString> checklistItems { get; set; }
            public IList<LookupString> riskHazardTypes { get; set; }
            public IList<LookupString> riskPeople { get; set; }
            public IList<LookupString> ethnicities { get; set; }
            public IList<LookupString> learnerRatingQuestions { get; set; }
        }
    }
