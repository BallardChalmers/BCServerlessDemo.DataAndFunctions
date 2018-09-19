using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{
    public class User
    {
        [JsonProperty(PropertyName = "objectId")]
        public string ObjectId { get; set; }
        [JsonProperty(PropertyName = "accountEnabled")]
        public bool AccountEnabled { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; } // This should hold the Org Id
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "creationType")]
        public string CreationType { get; set; }
        [JsonProperty(PropertyName = "displayName")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "givenName")]
        public string GivenName { get; set; }
        [JsonProperty(PropertyName = "immutableId")]
        public string ImmutableId { get; set; }
        [JsonProperty(PropertyName = "isCompromised")]
        public bool? IsCompromised { get; set; }
        [JsonProperty(PropertyName = "jobTitle")]
        public string JobTitle { get; set; }
        [JsonProperty(PropertyName = "mail")]
        public string Mail { get; set; }
        [JsonProperty(PropertyName = "mailNickName")]
        public string MailNickName { get; set; }
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "onPremisesDistinguishedName")]
        public string OnPremisesDistinguishedName { get; set; }
        [JsonProperty(PropertyName = "onPremisesSecurityIdentifier")]
        public string OnPremisesSecurityIdentifier { get; set; }
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "streetAddress")]
        public string StreetAddress { get; set; }
        [JsonProperty(PropertyName = "surname")]
        public string Surname { get; set; }
        [JsonProperty(PropertyName = "telephoneNumber")]
        public string TelephoneNumber { get; set; }
        [JsonProperty(PropertyName = "signInNames")]
        public List<SignInNames> SignInNames { get; set; }
        [JsonProperty(PropertyName = "userIdentities")]
        public List<UserIdentity> UserIdentities { get; set; }
        [JsonProperty(PropertyName = "userPrincipalName")]
        public string UserPrincipalName { get; set; }
        [JsonProperty(PropertyName = "userType")]
        public string UserType { get; set; }       
    }
}
