using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
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
    public interface IOrganisationsApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        // Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);
    }

    public class OrganisationsApi : IOrganisationsApi
    {
        private readonly IOrganisationService _organisationService;
        private readonly ISearchApi _searchApi;
        private readonly IUserDigestService _userDigestService;

        public OrganisationsApi(IOrganisationService organisationService,
           ISearchApi searchApi,
           IUserDigestService userDigestService)
        {
            _organisationService = organisationService;
            _searchApi = searchApi;
            _userDigestService = userDigestService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            string organisationId = req.GetQueryNameValuePairs().Where(w => w.Key == "id").FirstOrDefault().Value;

            if ((await _userDigestService.CurrentUserAsync(req)).AppRole == Role.Driver)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            if (string.IsNullOrWhiteSpace(organisationId))
            {
                var searchResults = await _searchApi.Search<Organisation, Organisation>(req, log);
                bool replaceUnapproved = false;
                bool.TryParse(req.GetQueryNameValuePairs().Where(w => w.Key == "replaceUnapproved").FirstOrDefault().Value, out replaceUnapproved);
                return req.CreateResponse(HttpStatusCode.OK, searchResults);
            }

            var org = await _organisationService.GetAsync(organisationId);
            return req.CreateResponse(HttpStatusCode.OK, org);
        }

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            var payload = await req.Content.ReadAsAsync<Organisation>();
            var item = await _organisationService.CreateAsync(payload, req);
            return req.CreateResponse(HttpStatusCode.Created, item);
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log)
        {
            var payload = await req.Content.ReadAsAsync<Organisation>();



            var item = await _organisationService.UpdateAsync(payload, req);
            return req.CreateResponse(HttpStatusCode.OK, item);
        }
    }
}
