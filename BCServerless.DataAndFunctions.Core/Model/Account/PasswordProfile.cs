using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{
    public class PasswordProfile
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [JsonProperty(PropertyName = "forceChangePasswordNextLogin")]
        public bool ForceChangePasswordNextLogin { get; set; }
    }
}
