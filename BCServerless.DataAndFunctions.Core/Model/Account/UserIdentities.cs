using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{
    public class UserIdentity
    {
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }
        [JsonProperty(PropertyName = "issuerUserId")]
        public string IssuerUserId { get; set; }
    }
}
