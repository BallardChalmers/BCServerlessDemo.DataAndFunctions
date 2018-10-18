using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Core.Service.Search;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IAppAdminDashboardOverviewApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);
    }

    public class AppAdminDashboardOverviewApi : IAppAdminDashboardOverviewApi
    {
        private readonly IDocumentDBRepository<Driver> _driverRepository;
        private readonly IDocumentDBRepository<Organisation> _organisationRepository;
        private readonly ISearchService _searchService;
        private readonly IUserDigestService _userDigestService;

        public AppAdminDashboardOverviewApi(IDocumentDBRepository<Driver> driverRepository,
            IDocumentDBRepository<Organisation> organisationRepository,
            ISearchService searchService,
            IUserDigestService userDigestService)
        {
            _driverRepository = driverRepository;
            _organisationRepository = organisationRepository;
            _searchService = searchService;
            _userDigestService = userDigestService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {

            var drivers = await _driverRepository.GetItemsAsync(t => t.deleted == false);
            var orgs = await _organisationRepository.GetItemsAsync(o => o.deleted == false);

            var userDigest = await _userDigestService.CurrentUserAsync(req);
            
            Organisation org = null;
            if (userDigest.AppRole == Role.Driver || userDigest.AppRole == Role.OrgAdmin)
            {
                org = await _organisationRepository.GetItemAsync(userDigest.OrganisationId);
            }

            var overview = new AppAdminDashboardOverview
            {
                driversNotExpiring = drivers.Count(),
                driversTotal = drivers.Count(),
                orgsNotExpiring = orgs.Count(),
                orgsTotal = orgs.Count(),
                orgId = userDigest.OrganisationId,
                orgName = org?.name,
                orgPhotoId = org?.PhotoId
            };

            return req.CreateResponse(HttpStatusCode.OK, overview);
        }
    }
}
