using Newtonsoft.Json;
using System.Collections.Generic;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{

    public class CreateUser
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
        public string organisationId { get; set; }
        public string organisationName { get; set; }
        public string AppRole { get; set; }
        public string AppRoleDisplayName { get; set; }
        public string driverId { get; set; }

        public B2CCreateUser GetB2CUser()
        {
            return new B2CCreateUser()
            {
                accountEnabled = accountEnabled,
                signInNames = signInNames,
                creationType = creationType,
                displayName = displayName,
                mailNickname = mailNickname,
                passwordProfile = passwordProfile,
                passwordPolicies = passwordPolicies,
                givenName = givenName,
                surname = surname
            };
        }

    }
}
