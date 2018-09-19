using System;
using Newtonsoft.Json;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Driver : BaseModel
    {
        [JsonProperty(PropertyName = "user")] public User RelatedUser { get; set; }
        [JsonProperty(PropertyName = "organisation")] public Organisation Organisation { get; set; }
        [JsonProperty(PropertyName = "joinedDate")] public DateTime JoinedDate { get; set; }
        [JsonProperty(PropertyName = "photoId")] public string PhotoId { get; set; }
        [JsonProperty(PropertyName = "isDisabled")] public bool IsDisabled { get; set; }
        [JsonProperty(PropertyName = "disableReason")] public string DisableReason { get; set; }
    }
}
