using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
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
    public interface IMetadataApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
        Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log);
    }

    public class MetadataApi : IMetadataApi
    {
        private readonly IDocumentDBRepository<Metadata> _metadataRepository;
        private readonly IUserDigestService _userDigestService;

        public MetadataApi(IDocumentDBRepository<Metadata> metadataRepository,
            IUserDigestService userDigestService)
        {
            _metadataRepository = metadataRepository;
            _userDigestService = userDigestService;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            if (!(await _userDigestService.IsAdminAsync(req)))
            {
                return req.CreateResponse(HttpStatusCode.Forbidden);
            }

            var item = await _metadataRepository.GetItemAsync(Metadata.MetadataId);
            return req.CreateResponse(HttpStatusCode.OK, item);
        }

        public async Task<HttpResponseMessage> Put(HttpRequestMessage req, TraceWriter log)
        {
            var payload = await req.Content.ReadAsAsync<Metadata>();

            SetIds(payload.checklistItems);
            SetIds(payload.riskHazardTypes);
            SetIds(payload.riskPeople);
            SetIds(payload.ethnicities);
            SetIds(payload.learnerRatingQuestions);

            var item = await _metadataRepository.UpdateItemAsync(payload.id, payload, req);

            return req.CreateResponse(HttpStatusCode.OK, item);
        }

        private void SetIds(IList<LookupString> list)
        {
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.id))
                {
                    item.id = Guid.NewGuid().ToString();
                }
            }
        }
    }
}
