using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnumsNET;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface IStartupApi : IHttpApi
    {
        Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log);
    }

    public class StartupApi : IStartupApi
    {
        private readonly IDocumentDBRepository<Config> _configRepo;
        private readonly ISampleDataApi _sampleDataApi;
        private readonly IDocumentDBRepository<Metadata> _metadataRepo;

        public StartupApi(IDocumentDBRepository<Config> configRepo,
            ISampleDataApi sampleDataApi,
            IDocumentDBRepository<Metadata> metadataRepo)
        {
            _configRepo = configRepo;
            _sampleDataApi = sampleDataApi;
            _metadataRepo = metadataRepo;
        }

        public async Task<HttpResponseMessage> Get(HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                await _configRepo.InitializeAsync();
            }
            catch (Exception ex)
            {
                log.Error($"Error initialising database: {ex.ToString()}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                await _sampleDataApi.SetupBaseData(req);
            }
            catch (Exception exp)
            {
                log.Error("Error setting base data: " + exp.Message);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            try
            {
                var metadata = await _metadataRepo.GetItemAsync(Metadata.MetadataId);

                var lookups = new Lookups()
                {
                    checklistItems = metadata.checklistItems,
                    riskHazardTypes = metadata.riskHazardTypes,
                    riskPeople = metadata.riskPeople,
                    ethnicities = metadata.ethnicities,
                    ratingQuestions = metadata.learnerRatingQuestions,
                    ragStatuses = Enums.GetMembers<RagStatus>().Select(x => new Lookup() { name = x.AsString(), value = x.ToInt32() }).ToList(),

                    certificationDate = Config.GetCertificationDate(DateTime.Now),
                    // TODO - Set these from an admin page
                    lowEvaluationLevel = 4,
                    highEvaluationLevel = 6
                };

                var config = await _configRepo.GetItemAsync(Config.ConfigId);

                return req.CreateResponse(HttpStatusCode.OK, lookups);
            }
            catch (Exception exp)
            {
                log.Error("Error setting metadata and lookups: " + exp.Message);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
