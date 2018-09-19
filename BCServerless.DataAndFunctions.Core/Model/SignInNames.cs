using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Model
{
    public class SignInNames
    {
        [JsonProperty(PropertyName = "type")] public string Type { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
