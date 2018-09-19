using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Data.Sample
{
    public static class SampleOrgs
    {
        public static List<Organisation> All => new List<Organisation>()
        {
            Organisation1, Organisation2
        };

        public static Organisation Organisation1 => new Organisation()
        {
            id = "1985651c-f63f-4bd0-9ebf-888f46376c66",
            name = "Test Org 1",
            primaryContact = new UserDB()
            {
                name = "Bob",
                mail = "bob@bobnet.com",
                telephoneNumber = "01234 567890"
            },
            proprietor = "Mr Org Proprietor"
        };

        public static Organisation Organisation2 => new Organisation()
        {
            id = "7fb4a13b-6d4f-4955-b09e-dc1ef1757e60",
            name = "Test Org 2",
            primaryContact = new UserDB()
            {
                name = "Jim"
            },
            proprietor = "Miss Org Proprietorese"
        };
    }
}