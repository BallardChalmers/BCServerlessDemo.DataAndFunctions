using System;
using Newtonsoft.Json;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Driver : BaseModel
    {
        [JsonProperty(PropertyName = "givenName")] public string GivenName { get; set; }
        [JsonProperty(PropertyName = "surname")] public string Surname { get; set; }
        [JsonProperty(PropertyName = "licenseNumber")] public string LicenseNumber { get; set; }
        [JsonProperty(PropertyName = "description")] public string Description { get; set; }
        [JsonProperty(PropertyName = "user")] public User RelatedUser { get; set; }
        [JsonProperty(PropertyName = "organisation")] public Organisation Organisation { get; set; }
        [JsonProperty(PropertyName = "expirationDate")] public DateTime ExpirationDate { get; set; }
        [JsonProperty(PropertyName = "photoId")] public string PhotoId { get; set; }
        [JsonProperty(PropertyName = "isDisabled")] public bool IsDisabled { get; set; }
        [JsonProperty(PropertyName = "disableReason")] public string DisableReason { get; set; }
        [JsonProperty(PropertyName = "statusCode")] public string StatusCode { get; set; }
        [JsonProperty(PropertyName = "statusDescription")] public string StatusDescription { get; set; }
        [JsonProperty(PropertyName = "lastUpdated")] public DateTime LastUpdated { get; set; }

    }
}
