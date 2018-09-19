using EnumsNET;
using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IUserDigestService
    {
        Task<UserDigest> CurrentUserAsync(HttpRequestMessage req);
        Task<bool> IsAdminAsync(HttpRequestMessage req);
    }

    public class UserDigestService : IUserDigestService
    {
        private readonly IUserDBRepository _userRepository;

        public UserDigestService(IUserDBRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDigest> CurrentUserAsync(HttpRequestMessage req)
        {
            var userIssuer = ClaimsPrincipal.Current.Claims.Where(f => f.Type.Contains("name")).FirstOrDefault();

            if (userIssuer != null && userIssuer.Issuer.ToUpper() == "LOCAL AUTHORITY")
            {
                if (req != null)
                {
                    IEnumerable<string> ids = new List<string>();
                    req.Headers.TryGetValues("UserId", out ids);
                    if (ids != null && ids.Any())
                    {
                        var userId = ids.First();
                        var user = await _userRepository.GetItemAsync(userId);
                        if (user != null)
                        {
                            return new UserDigest()
                            {
                                Id = user.id,
                                DisplayName = user.displayName,
                                OrganisationId = user.organisationId,
                                AppRole = user.AppRole == null ? Role.Driver : Enums.Parse<Role>(user.AppRole),
                                DriverId= user.driverId
                            };
                        }
                    }
                }

                var userDigest = new UserDigest()
                {
                    Id = "0cdde2ed-04e4-47d9-95b1-23df587a03b6",
                    DisplayName = "Test Account",
                    OrganisationId = "admin",
                    AppRole = Role.Admin,
                    //OrganisationId = "1985651c-f63f-4bd0-9ebf-888f46376c66",
                    //AppRole = Role.TrainingProviderAdmin,
                    //DriverId= "7051831e-3ec9-4615-bb6f-a8b0bf709b7e"
                };
                return userDigest;
            }
            else
            {
                
                var objectId = ClaimsPrincipal.Current.Claims.Where(f => f.Type.Contains("nameidentifier")).FirstOrDefault().Value.ToString();
                //var objectId = ClaimsPrincipal.Current.Claims.First(f => f.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                var user = await _userRepository.GetItemAsync(objectId);
                return new UserDigest()
                {
                    Id = user.id,
                    DisplayName = user.displayName,
                    OrganisationId = user.organisationId,
                    AppRole = Enums.Parse<Role>(user.AppRole),
                    DriverId= user.driverId
                };
            }
        }

        public async Task<bool> IsAdminAsync(HttpRequestMessage req)
        {
            var userDigest = await CurrentUserAsync(req);
            return userDigest.AppRole == Role.Admin || userDigest.AppRole == Role.Manager;
        }
    }
}
