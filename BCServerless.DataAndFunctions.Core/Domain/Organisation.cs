using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain
{
    public class Organisation : BaseModel
    {
        [JsonProperty(PropertyName = "photoId")] public string PhotoId { get; set; }
        [JsonProperty(PropertyName = "description")] public string Description { get; set; }
        public UserDB primaryContact { get; set; }
        public string proprietor { get; set; }
    }
}
