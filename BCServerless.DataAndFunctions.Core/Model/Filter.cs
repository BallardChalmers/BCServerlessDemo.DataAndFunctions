using System;
using System.Collections.Generic;
using BCServerlessDemo.DataAndFunctions.Core.Domain;

namespace BCServerlessDemo.DataAndFunctions.Core.Model
{
    public interface ICardsIssuedFilter
    {
        int? IssuedWithinDays { get; set; }
        int? IssuedYear { get; set; }
        IEnumerable<string> OrganisationNames { get; set; }
    }

    public class Filter : ICardsIssuedFilter
    {
        public IEnumerable<BaseModel> Offices { get; set; }
        public IEnumerable<BaseModel> AuthorisedPersons { get; set; }
        public IEnumerable<BaseModel> Occupations { get; set; }
        public IEnumerable<BaseModel> Organisations { get; set; }
        public IEnumerable<string> NVQCertificateTitles { get; set; }
        public DateTime? FromDateCardExpiry { get; set; }
        public DateTime? ToDateCardExpiry { get; set; }
        public DateTime? FromDateBirth { get; set; }
        public DateTime? ToDateBirth { get; set; }
        public DateTime? FromDateNVQCertExpiry { get; set; }
        public DateTime? ToDateNVQCertExpiry { get; set; }
        public string OrganisationId { get; set; }
        public DateTime? FromDateEvidenceExpiry { get; set; }
        public DateTime? ToDateEvidenceExpiry { get; set; }
        public bool? PhotosIsNull { get; set; }
        public bool? HasExpiredDocs { get; set; }
        public int? IssuedWithinDays { get; set; }
        public int? IssuedYear { get; set; }
        public IEnumerable<string> OrganisationNames { get; set; }
    }
}
