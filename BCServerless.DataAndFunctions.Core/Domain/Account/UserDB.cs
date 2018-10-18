using System.Collections.Generic;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using Newtonsoft.Json;

namespace BCServerlessDemo.DataAndFunctions.Core.Domain.Account
{
    public class UserDB : BaseModel
    {
        public string objectId { get; set; }
        public string organisationId { get; set; }
        public string organisationName { get; set; }
        public string appRole { get; set; }
        public string appRoleDisplayName { get; set; }
        public string driverId { get; set; }
        public bool accountEnabled { get; set; }
        public string city { get; set; }
        public string companyName { get; set; }
        public string country { get; set; }   
        public string creationType { get; set; }
        public string displayName { get; set; }  
        public string givenName { get; set; }
        public string immutableId { get; set; }   
        public bool? isCompromised { get; set; }
        public string jobTitle { get; set; }
        public string mail { get; set; }
        public string mailNickName { get; set; }
        public string mobile { get; set; }

        public string onPremisesDistinguishedName { get; set; }
        public string onPremisesSecurityIdentifier { get; set; }
        public string postalCode { get; set; }
        public string state { get; set; }
        public string streetAddress { get; set; }
        public string surname { get; set; }
        public string telephoneNumber { get; set; }
        public List<SignInNames> signInNames { get; set; }
        public List<UserIdentity> userIdentities { get; set; }
        public string userPrincipalName { get; set; }
        public string userType { get; set; }

        public UserDB()
        {

        }

        public UserDB(User user)
        {
            id = user.ObjectId;
            objectId = user.ObjectId;
            accountEnabled = user.AccountEnabled;
            city = user.City;
            companyName = user.CompanyName;
            country = user.Country;
            creationType = user.CreationType;
            displayName = user.DisplayName;
            givenName = user.GivenName;
            immutableId = user.ImmutableId;
            isCompromised = user.IsCompromised;
            jobTitle = user.JobTitle;
            mail = user.Mail;
            mailNickName = user.MailNickName;
            mobile = user.Mobile;
            onPremisesDistinguishedName = user.OnPremisesDistinguishedName;
            onPremisesSecurityIdentifier = user.OnPremisesSecurityIdentifier;
            postalCode = user.PostalCode;
            state = user.State;
            streetAddress = user.StreetAddress;
            surname = user.Surname;
            telephoneNumber = user.TelephoneNumber;
            signInNames = user.SignInNames;
            userIdentities = user.UserIdentities;
            userPrincipalName = user.UserPrincipalName;
            userType = user.UserType;
        }

        public User GetUser()
        {
            User user = new User()
            {
                ObjectId = id,
                AccountEnabled = accountEnabled,
                City = city,
                CompanyName = companyName,
                Country = country,
                CreationType = creationType,
                DisplayName = displayName,
                GivenName = givenName,
                ImmutableId = immutableId,
                IsCompromised = isCompromised,
                JobTitle = jobTitle,
                Mail = mail,
                MailNickName = mailNickName,
                Mobile = mobile,
                OnPremisesDistinguishedName = onPremisesDistinguishedName,
                OnPremisesSecurityIdentifier = onPremisesSecurityIdentifier,
                PostalCode = postalCode,
                State = state,
                StreetAddress = streetAddress,
                Surname = surname,
                TelephoneNumber = telephoneNumber,
                SignInNames = signInNames,
                UserIdentities = userIdentities,
                UserPrincipalName = userPrincipalName,
                UserType = userType 
            };

            if (user.Mail == null)
            {
                if (user.SignInNames != null && user.SignInNames.Count > 0)
                {
                    user.Mail = user.SignInNames[0].Value;
                }
            }

            return user;
        }
    }
}
