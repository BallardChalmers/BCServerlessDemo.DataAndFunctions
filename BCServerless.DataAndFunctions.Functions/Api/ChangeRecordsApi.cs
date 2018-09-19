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
        public interface IChangeRecordsApi : IHttpApi
        {
            Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
            // Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
            // Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
            // Task<HttpResponseMessage> Patch(HttpRequestMessage req, TraceWriter log);
            // Task<HttpResponseMessage> Delete(HttpRequestMessage req, TraceWriter log);  
        }

        public class ChangeRecordsApi : IChangeRecordsApi
        {
            private readonly IUserDigestService _userDigestService;
            private readonly IDocumentDBRepository<ChangeRecord> _changeRecordRepository;
            private readonly ISearchApi _searchApi;

            public ChangeRecordsApi(IUserDigestService userDigestService,
                IDocumentDBRepository<ChangeRecord> changeRecordRepository,
                ISearchApi searchApi)
            {
                _userDigestService = userDigestService;
                _changeRecordRepository = changeRecordRepository;
                _searchApi = searchApi;
            }

            public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
            {
                if (!await _userDigestService.IsAdminAsync(req))
                {
                    return req.CreateResponse(HttpStatusCode.Forbidden);
                }

                var id = req.GetQueryNameValuePairs().Where(w => w.Key == "id").FirstOrDefault().Value;

                if (id != null)
                {
                    var changeRecord = await _changeRecordRepository.GetItemAsync(id);
                    return req.CreateResponse(HttpStatusCode.OK, changeRecord);
                }
                else
                {
                    var results = await _searchApi.Search<ChangeRecord, ChangeRecord>(req, log);
                    return req.CreateResponse(HttpStatusCode.OK, results);
                }
            }
        }
    }
