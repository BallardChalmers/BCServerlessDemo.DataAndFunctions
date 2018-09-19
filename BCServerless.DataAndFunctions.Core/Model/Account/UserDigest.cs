using BCServerlessDemo.DataAndFunctions.Core.Domain;

namespace BCServerlessDemo.DataAndFunctions.Core.Model.Account
{
    public class UserDigest
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string OrganisationId { get; set; }
        public Role AppRole { get; set; } 
        public string DriverId { get; set; }
    }
}
