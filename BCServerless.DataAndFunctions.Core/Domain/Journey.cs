using System;
using Newtonsoft.Json;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Journey: BaseModel
    {
        [JsonProperty(PropertyName = "driver")] public Driver Driver { get; set; }
        [JsonProperty(PropertyName = "vehicle")] public Vehicle Vehicle { get; set; }
        [JsonProperty(PropertyName = "startLocationName")] public string StartLocationName { get; set; }
        [JsonProperty(PropertyName = "endLocationName")] public string EndLocationName { get; set; }
        [JsonProperty(PropertyName = "journeyDate")] public DateTime JourneyDate { get; set; }
    }
}
