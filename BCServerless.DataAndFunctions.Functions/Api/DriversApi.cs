using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IDriversApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);
    }

    public class DriversApi : IDriversApi
    {
        private readonly IUserDigestService _userDigestService;
        private readonly IDocumentDBRepository<Driver> _DriverRepository;
        private readonly ISearchApi _searchApi;  
        private readonly IDriverService _DriverService;

        public DriversApi(IUserDigestService userDigestService,
           IDocumentDBRepository<Driver> DriverRepository,
           ISearchApi searchApi,
           IUserService userService,
           IDriverService DriverService)
        {
            _userDigestService = userDigestService;
            _DriverRepository = DriverRepository;  
            _searchApi = searchApi;
            _DriverService = DriverService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            string id = req.GetQueryNameValuePairs().Where(w => w.Key == "id").FirstOrDefault().Value;           
           
            if (string.IsNullOrWhiteSpace(id))
            {
                var results = await _searchApi.Search<Driver, Driver>(req, log);

                bool replaceUnapproved = false;
                bool.TryParse(req.GetQueryNameValuePairs().Where(w => w.Key == "replaceUnapproved").FirstOrDefault().Value, out replaceUnapproved);


                return req.CreateResponse(HttpStatusCode.OK, results);
            }           

            var item = await _DriverRepository.GetItemAsync(id);
            return req.CreateResponse(HttpStatusCode.OK, item);
        }    

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            if ((await _userDigestService.CurrentUserAsync(req)).AppRole == Role.Driver)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var payload = await req.Content.ReadAsAsync<Driver>();
            var item = await _DriverService.CreateAsync(payload, req);            

            return req.CreateResponse(HttpStatusCode.Created, item);
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log)
        {
            if ((await _userDigestService.CurrentUserAsync(req)).AppRole == Role.Driver)
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }
            var Driver = await req.Content.ReadAsAsync<Driver>();            

            var item = await _DriverService.UpdateAsync(Driver, req);

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

            var Driver = await _DriverRepository.GetItemAsync(id);
            if (Driver == null)
            {
                return req.CreateResponse(HttpStatusCode.NotFound);
            }

            Driver = await _DriverService.DeleteAsync(Driver, req);

            return req.CreateResponse(HttpStatusCode.OK, Driver);
        }
    }
}
