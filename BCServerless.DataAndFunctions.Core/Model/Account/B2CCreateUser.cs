using Newtonsoft.Json;
using System.Collections.Generic;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{
    //Note - Can only include properties recognised by the B2C provider
    public class B2CCreateUser
    {
        public bool accountEnabled { get; set; }
        public List<SignInNames> signInNames { get; set; }  
        public string creationType { get; set; }
        public string displayName { get; set; }
        public string mailNickname { get; set; }
        public PasswordProfile passwordProfile { get; set; }
        public string passwordPolicies { get; set; }
        public string givenName { get; set; }   
        public string surname { get; set; }
    }
}
