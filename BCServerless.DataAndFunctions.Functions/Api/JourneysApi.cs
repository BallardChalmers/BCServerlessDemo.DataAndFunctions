using BCServerlessDemo.DataAndFunctions.Functions.Api;
using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IJourneysApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);  
    }

    public class JourneysApi : IJourneysApi
    {
        private readonly IDocumentDBRepository<Journey> _JourneyRepository;
        private readonly IJourneyService _JourneyService;
        private readonly ISearchApi _searchApi;
        private readonly IUserDigestService _userDigestService;

        public JourneysApi(IDocumentDBRepository<Journey> JourneyRepository,
            IJourneyService JourneyService,
           ISearchApi searchApi,
           IUserDigestService userDigestService)
        {
            _JourneyRepository = JourneyRepository;
            _JourneyService = JourneyService;
            _searchApi = searchApi;
            _userDigestService = userDigestService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            string idString = req.GetQueryNameValuePairs().Where(w => w.Key == "id").FirstOrDefault().Value;

            if (string.IsNullOrWhiteSpace(idString))
            {
                var results = await _searchApi.Search<Journey, Journey>(req, log);
                return req.CreateResponse(HttpStatusCode.OK, results);
            }
            
            var Journey = await _JourneyService.Get(idString);
            if (Journey == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            return req.CreateResponse(HttpStatusCode.OK, Journey);
        }       

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            var payload = await req.Content.ReadAsAsync<Journey>(); 
            var item = await _JourneyService.CreateAsync(payload, req);

            return req.CreateResponse(HttpStatusCode.Created, item);
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log)
        {
            var Journey = await req.Content.ReadAsAsync<Journey>(); 
            var item = await _JourneyService.UpdateAsync(Journey, req);

            return req.CreateResponse(HttpStatusCode.OK, item);
        }

        public async Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log)
        {
            string id = req.GetQueryNameValuePairs().Where(w => w.Key == "id").FirstOrDefault().Value;

            if ((await _userDigestService.CurrentUserAsync(req)).AppRole == Role.Driver)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            if (string.IsNullOrWhiteSpace(id))
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            var Journey = await _JourneyRepository.GetItemAsync(id);
            if (Journey == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            Journey = await _JourneyService.DeleteAsync(Journey, req);

            return req.CreateResponse(HttpStatusCode.OK, Journey);
        }
    }
}
