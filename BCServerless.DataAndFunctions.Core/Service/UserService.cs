using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IUserService
    {
        Task<UserDB> GetUserAsync(string userId, HttpRequestMessage req);
        Task<IList<UserDB>> GetAllUsers(HttpRequestMessage req);
        Task<IList<UserDB>> GetByOrgAsync(string organisationId);
        Task<IList<UserDB>> GetByDriverAsync(string driverId);
        Task<UserDB> CreateUserAsync(CreateUser payload, TraceWriter log, HttpRequestMessage req);
        Task<UserDB> UpdateUserAsync(UserDB user, HttpRequestMessage req);
        Task<IList<UserDB>> SearchAsync(string searchTerm, HttpRequestMessage req);

    }

    public class UserService : IUserService
    {
        const string _aadGraphResourceId = "https://graph.windows.net/";
        const string _aadGraphEndpoint = "https://graph.windows.net/";
        const string _aadGraphVersion = "api-version=1.6";
        const string _authorityEndpoint = "https://login.microsoftonline.com/";

        private readonly IUserDBRepository _userRepository;
        private readonly IApplicationConfig _applicationConfig;
        private readonly IUserDigestService _userDigestService;

        public UserService(IUserDBRepository userRepository,
           IApplicationConfig applicationConfig,
           IUserDigestService userDigestService)
        {

            _userRepository = userRepository;
            _applicationConfig = applicationConfig;
            _userDigestService = userDigestService;
        }

        public async Task<UserDB> GetUserAsync(string userId, HttpRequestMessage req)
        {
            var tenant = _applicationConfig.B2CTenant;
            var clientID = _applicationConfig.B2CClient;
            var secret = _applicationConfig.B2CSecret;

            //  Ceremony
            var authority = $"{_authorityEndpoint}{tenant}";
            var authContext = new AuthenticationContext(authority);
            var credentials = new ClientCredential(clientID, secret);

            AuthenticationResult result = await authContext.AcquireTokenAsync(_aadGraphResourceId, credentials);
            HttpClient http = new HttpClient();
            string url = _aadGraphEndpoint + tenant + "/users/" + userId + "?" + _aadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage sentResponse = await http.SendAsync(request);
            string responseText = await sentResponse.Content.ReadAsStringAsync();

            var allUsers = new List<UserDB>();
            
            var b2cUser = JsonConvert.DeserializeObject<User>(responseText);
            var singleUser = await _userRepository.GetItemAsync(b2cUser.ObjectId) ?? await CreateNewUser(b2cUser, req);         
            return singleUser;          
        }

        public async Task<IList<UserDB>> GetAllUsers(HttpRequestMessage req)
        {
            var tenant = _applicationConfig.B2CTenant;
            var clientID = _applicationConfig.B2CClient;
            var secret = _applicationConfig.B2CSecret;

            //  Ceremony
            var authority = $"{_authorityEndpoint}{tenant}";
            var authContext = new AuthenticationContext(authority);
            var credentials = new ClientCredential(clientID, secret);

            AuthenticationResult result = await authContext.AcquireTokenAsync(_aadGraphResourceId, credentials);
            HttpClient http = new HttpClient();
            string url = _aadGraphEndpoint + tenant + "/users?" + _aadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage sentResponse = await http.SendAsync(request);
            string responseText = await sentResponse.Content.ReadAsStringAsync();

            var allUsers = new List<UserDB>();           
            
            JObject allUsersjson = JObject.Parse(responseText);
            foreach (var user in allUsersjson["value"])
            {
                var b2cUser = user.ToObject<User>();
                var singleUser = await _userRepository.GetItemAsync(b2cUser.ObjectId) ?? await CreateNewUser(b2cUser, req);
                allUsers.Add(singleUser);
            }

            return allUsers;            
        }

        public async Task<IList<UserDB>> GetByOrgAsync(string organisationId)
        {
            var orgUsers = await _userRepository.GetItemsAsync(i => i.organisationId == organisationId);
            return orgUsers.ToList();           
        }

        public async Task<IList<UserDB>> GetByDriverAsync(string driverId)
        {
            var users = await _userRepository.GetItemsAsync(i => i.driverId == driverId);
            return users.ToList();
        }

        public async Task<UserDB> CreateUserAsync(CreateUser payload, TraceWriter log, HttpRequestMessage req)
        {
            var tenant = _applicationConfig.B2CTenant;
            var clientID = _applicationConfig.B2CClient;
            var secret = _applicationConfig.B2CSecret;

            //  Ceremony
            var authority = $"{_authorityEndpoint}{tenant}";
            var authContext = new AuthenticationContext(authority);
            var credentials = new ClientCredential(clientID, secret);

            AuthenticationResult result = await authContext.AcquireTokenAsync(_aadGraphResourceId, credentials);
            HttpClient http = new HttpClient();
            string url = _aadGraphEndpoint + tenant + "/users?" + _aadGraphVersion;
            HttpRequestMessage adRequest = new HttpRequestMessage(HttpMethod.Post, url);

            var userString = JsonConvert.SerializeObject(payload.GetB2CUser());
            adRequest.Content = new StringContent(userString, Encoding.UTF8, "application/json");
            adRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage adResponse = await http.SendAsync(adRequest);
            string responseText = await adResponse.Content.ReadAsStringAsync();

            if (adResponse.IsSuccessStatusCode == false)
            {
                log.Error($"Error in AD response {adResponse.StatusCode}: {responseText}");
                return null;
            }

            var b2CUser = JObject.Parse(responseText).ToObject<User>();
            var newUser = new UserDB(b2CUser);
            newUser.organisationId = payload.organisationId;
            newUser.organisationName = payload.organisationName;
            newUser.appRole = payload.AppRole;
            newUser.appRoleDisplayName = payload.AppRoleDisplayName;
            newUser.driverId = payload.driverId;

            return await _userRepository.CreateItemAsync(newUser, _userDigestService, req);
        }

        public async Task<UserDB> UpdateUserAsync(UserDB user, HttpRequestMessage req)
        {
            //TODO: add security to ensure that calling user has access to do this
            var tenant = _applicationConfig.B2CTenant;
            var clientID = _applicationConfig.B2CClient;
            var secret = _applicationConfig.B2CSecret;

            //  Ceremony
            var authority = $"{_authorityEndpoint}{tenant}";
            var authContext = new AuthenticationContext(authority);
            var credentials = new ClientCredential(clientID, secret);

            AuthenticationResult result = await authContext.AcquireTokenAsync(_aadGraphResourceId, credentials);
            HttpClient http = new HttpClient();
            string url = _aadGraphEndpoint + tenant + "/users/" + user.userPrincipalName + "?" + _aadGraphVersion;
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            var userString = JsonConvert.SerializeObject(user.GetUser());
            request.Content = new StringContent(userString, Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            var httpResponse = await http.SendAsync(request);
            string responseText = await httpResponse.Content.ReadAsStringAsync();

            //TODO: handle if db update fails when graph update succeeded 
            var matchingUsers = await _userRepository.GetItemsAsync(matching => matching.id == user.id);

            if (matchingUsers.Count() > 0)
            {
                return await _userRepository.UpdateItemAsync(user.objectId, user, _userDigestService, req);
            }
            else
            {
                return await _userRepository.CreateItemAsync(user, _userDigestService, req);
            }
        }

        private async Task<UserDB> CreateNewUser(User b2cUser, HttpRequestMessage req)
        {
            var user = new UserDB(b2cUser);
            if (_applicationConfig.AssignNewUserAdminRole)
            {
                user.appRole = Role.Admin.ToString();
            }

            return await _userRepository.CreateItemAsync(user, _userDigestService, req);
        }

        // Note - Move into a UserQueryBuilder if any more terms need to be searched for
        public async Task<IList<UserDB>> SearchAsync(string searchTerm, HttpRequestMessage req)
        {
            return (await _userRepository.GetItemsAsync(u => u.displayName.ToLower().Contains(searchTerm.ToLower()))).ToList();
        }
    }
}
