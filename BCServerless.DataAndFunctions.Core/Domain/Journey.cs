using System;
using Newtonsoft.Json;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Journey: BaseModel
    {
        //[JsonProperty(PropertyName = "driver")] public Driver Driver { get; set; }
        //[JsonProperty(PropertyName = "vehicle")] public Vehicle Vehicle { get; set; }
        [JsonProperty(PropertyName = "pickupLongitude")] public float PickupLongitude { get; set; }
        [JsonProperty(PropertyName = "pickupLatitude")] public float PickupLatitude { get; set; }
        [JsonProperty(PropertyName = "pickupAddress")] public Address PickupAddress { get; set; }
        [JsonProperty(PropertyName = "dropoffLongitude")] public float DropoffLongitude { get; set; }
        [JsonProperty(PropertyName = "dropoffLatitude")] public float DropoffLatitude { get; set; }
        [JsonProperty(PropertyName = "dropoffAddress")] public Address DropoffAddress { get; set; }
        [JsonProperty(PropertyName = "pickupDate")] public DateTime PickupDate { get; set; }
        [JsonProperty(PropertyName = "dropoffDate")] public DateTime DropoffDate { get; set; }
        [JsonProperty(PropertyName = "passengerCount")] public int PassengerCount { get; set; }
        [JsonProperty(PropertyName = "journeyDuration")] public int JourneyDuration { get; set; }

        public Journey(DateTime PickupDate, DateTime DropoffDate,
            int PassengerCount, float PickupLongitude, float PickupLatitude,
            float DropoffLongitude, float DropoffLatitude, int JourneyDuration)
        {
            this.id = Guid.NewGuid().ToString();
            this.PickupDate = PickupDate;
            this.DropoffDate = DropoffDate;
            this.PassengerCount = PassengerCount;
            this.PickupLongitude = PickupLongitude;
            this.PickupLatitude = PickupLatitude;
            this.DropoffLongitude = DropoffLongitude;
            this.DropoffLatitude = DropoffLatitude;
            this.JourneyDuration = JourneyDuration;
        }
    }
}
