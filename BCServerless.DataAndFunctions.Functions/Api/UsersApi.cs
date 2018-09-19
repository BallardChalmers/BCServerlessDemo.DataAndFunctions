using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Domain.Account;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IUsersApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
    }

    public class UsersApi : IUsersApi
    {
        private readonly IUserDigestService _userDigestService;
        private readonly IUserService _userService;
        private readonly IDocumentDBRepository<Driver> _driverRepository;

        public UsersApi(IUserDigestService userDigestService,
           IUserService userService,
           IDocumentDBRepository<Driver> driverRepository)
        {
            _userDigestService = userDigestService;
            _userService = userService;
            _driverRepository = driverRepository;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                var organisationId = req.GetQueryNameValuePairs().Where(w => w.Key == "organisationId").FirstOrDefault().Value;

                bool isAdmin = await _userDigestService.IsAdminAsync(req);
                var userDigest = await _userDigestService.CurrentUserAsync(req);
                if (!string.IsNullOrEmpty(organisationId) && (isAdmin || userDigest.OrganisationId == organisationId))
                {
                    var orgUsers = await _userService.GetByOrgAsync(organisationId);
                    return req.CreateResponse(HttpStatusCode.OK, orgUsers.ToList());
                }

                var userId = req.GetQueryNameValuePairs().Where(w => w.Key == "userId").FirstOrDefault().Value;

                var allUsers = new List<UserDB>();
                if (!string.IsNullOrEmpty(userId))
                {
                    var singleUser = await _userService.GetUserAsync(userId, req);
                    allUsers.Add(singleUser);
                }
                else
                {
                    var searchTerm = req.GetQueryNameValuePairs().Where(w => w.Key == "SearchTerm").FirstOrDefault().Value;
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        allUsers.AddRange(await _userService.SearchAsync(searchTerm, req));
                    }
                    else
                    {
                        var users = await _userService.GetAllUsers(req);
                        allUsers.AddRange(users);
                    }
                }
                return req.CreateResponse(HttpStatusCode.OK, allUsers);
            }
            catch(Exception exp)
            {
                log.Error("Error retrieving users: " + exp.Message + exp.StackTrace);
                return req.CreateErrorResponse(HttpStatusCode.InternalServerError, exp);
            }
        }

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            bool isAdmin = await _userDigestService.IsAdminAsync(req);
            if (isAdmin == false)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            HttpResponseMessage response;
            try
            {
                var payload = await req.Content.ReadAsAsync<CreateUser>();
                var user = await _userService.CreateUserAsync(payload, log, req);
                if (user == null)
                {
                    req.CreateResponse(HttpStatusCode.InternalServerError, "Unable to create user");
                }
                response = req.CreateResponse(HttpStatusCode.Created, user); 
            }
            catch (Exception ex)
            {
                log.Error("Error - user", ex);
                response = req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log)
        {
            bool isAdmin = await _userDigestService.IsAdminAsync(req);
            if (isAdmin == false)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            HttpResponseMessage response;

            try
            {                
                var payload = await req.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<UserDB>(payload);

                if (user.userPrincipalName == null)
                {
                    req.CreateResponse(HttpStatusCode.BadRequest);
                }

                //Sync user.accountEnabled and driver.isDisabled flags
                var currentUser = await _userService.GetUserAsync(user.id, req);
                if (currentUser.accountEnabled != user.accountEnabled && !string.IsNullOrWhiteSpace(user.driverId))
                {
                    var driver = await _driverRepository.GetItemAsync(user.driverId);
                    bool isDisabled = !user.accountEnabled;
                    if (driver != null && driver.IsDisabled != isDisabled)
                    {
                        driver.IsDisabled = isDisabled;
                        driver.DisableReason = "User account disabled";
                        await _driverRepository.UpdateItemAsync(driver.id, driver, req);
                    }
                }

                user = await _userService.UpdateUserAsync(user, req); 
                response = req.CreateResponse(HttpStatusCode.OK, user);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                                
            }
            catch (Exception ex)
            {
                log.Error("Error - user", ex);
                response = req.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
