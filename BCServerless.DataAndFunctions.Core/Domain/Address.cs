using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Address
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }
        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }
        [JsonProperty(PropertyName = "townCity")]
        public string TownCity { get; set; }
        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }
        [JsonProperty(PropertyName = "postcode")]
        public string Postcode { get; set; }
    }
}
