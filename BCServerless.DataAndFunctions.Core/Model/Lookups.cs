using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;

namespace BCServerlessDemo.DataAndFunctions.Core.Model
{
    public class Lookups
    {
        public Lookups()
        {
            auditTypes = new List<Lookup>();
            insuranceTypes = new List<Lookup>();
            journeyTypes = new List<Lookup>();
            markedOptions = new List<Lookup>();
            approvalItemTypes = new List<Lookup>();
            approvalActions = new List<Lookup>();
            approvalStatuses = new List<Lookup>();
            checklistItems = new List<LookupString>();
            riskHazardTypes = new List<LookupString>();
            riskPeople = new List<LookupString>();
            ethnicities = new List<LookupString>();     
            ratingQuestions = new List<LookupString>();
            ragStatuses = new List<Lookup>();
        }

        public IList<Lookup> auditTypes { get; set; }     
        public IList<Lookup> insuranceTypes { get; set; }
        public IList<Lookup> journeyTypes { get; set; }
        public IList<Lookup> markedOptions { get; set; }
        public IList<Lookup> approvalItemTypes { get; set; }
        public IList<Lookup> approvalActions { get; set; }    
        public IList<Lookup> approvalStatuses { get; set; }
        public IList<LookupString> checklistItems { get; set; }
        public IList<LookupString> riskHazardTypes { get; set; }
        public IList<LookupString> riskPeople { get; set; }
        public IList<LookupString> ethnicities { get; set; }   
        public IList<LookupString> ratingQuestions { get; set; }
        public IList<Lookup> ragStatuses { get; set; }
        public int lowEvaluationLevel { get; set; }
        public int highEvaluationLevel { get; set; }
        public DateTime certificationDate { get; set; }
        public bool journeyApprovalsEnabled { get; set; }
    }
}
