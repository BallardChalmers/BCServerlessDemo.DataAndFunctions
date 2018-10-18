using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleUsers
    {
        public static UserDB DriverAdmin => new UserDB()
        {
            objectId = "ae722b5f-f122-4d6d-8492-5be9fa543727",
            organisationId = "1985651c-f63f-4bd0-9ebf-888f46376c66",
            organisationName = "Test Org 1",
            appRole = "TrainingProviderAdmin",
            appRoleDisplayName = "Training Provider Admin",
            driverId = "7051831e-3ec9-4615-bb6f-a8b0bf709b7e",
            accountEnabled = true,
            creationType = "LocalAccount",
            displayName = "Test",
            givenName = "Test",
            surname = "User",
            signInNames = new List<SignInNames>() {
                new SignInNames() {
                Type = "emailAddress",
                Value = "testuser@ballardchalmers.com"
            }
            },
            userPrincipalName = "8f965872-30a0-484c-b70a-6b7c8c3c0468@bcserverlessdemo.onmicrosoft.com",
            userType = "Member",
            id = "ae722b5f-f122-4d6d-8492-5be9fa543727",
        };
    }
}
