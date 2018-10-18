using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Data.Base;
using BCServerlessDemo.DataAndFunctions.Core.Data.Sample;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.Api
{
    public interface ISampleDataApi : IHttpApi
    {
        Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log);
        Task SetupBaseData(HttpRequestMessage req);
    }

    public class SampleDataApi : ISampleDataApi
    {
        private readonly IJourneyService _journeyService;
        private readonly IDocumentDBRepository<Journey> _journeyRepository;
        private readonly IDocumentDBRepository<Organisation> _orgRepository;
        private readonly IDocumentDBRepository<Config> _configRepo;
        private readonly IDocumentDBRepository<Driver> _driverRepo;
        private readonly IDocumentDBRepository<Metadata> _metadataRepository;
        private readonly IDocumentDBRepository<EventReminder> _eventReminderRepo;
        private readonly IDocumentDBRepository<Vehicle> _vehicleRepo;
        private readonly IUserService _userService;
        private readonly IUserDBRepository _userDBRepository;
        private readonly IUserDigestService _userDigestService;
        private readonly IFileService _fileService;

        public SampleDataApi(
            IJourneyService journeyService,
            IDocumentDBRepository<Journey> journeyRepository,
            IDocumentDBRepository<Organisation> orgRepository,
            IDocumentDBRepository<Config> configRepo,
            IDocumentDBRepository<Driver> driverRepo,
            IDocumentDBRepository<Metadata> metadataRepository,
            IDocumentDBRepository<EventReminder> eventReminderRepo,
            IDocumentDBRepository<Vehicle> vehicleRepo,
            IFileService fileService,
            IUserService userService,
            IUserDBRepository userDBRepository,
            IUserDigestService userDigestService)
        {
            _journeyService = journeyService;
            _journeyRepository = journeyRepository;
            _orgRepository = orgRepository;
            _configRepo = configRepo;
            _driverRepo = driverRepo;
            _metadataRepository = metadataRepository;
            _eventReminderRepo = eventReminderRepo;
            _vehicleRepo = vehicleRepo;
            _fileService = fileService;
            _userService = userService;
            _userDBRepository = userDBRepository;
            _userDigestService = userDigestService;
        }

        public async Task<HttpResponseMessage> Post(HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                await _journeyRepository.InitializeAsync();
            }
            catch (Exception ex)
            {
                log.Error($"Error initialising database: {ex.ToString()}");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            await SetupBaseData(req);

            await UpdateUserRoles(req);

            await UpdateUserRoles(req);

            // var sampleFile = await AddSampleFile(req);

            foreach (var journey in SampleJourneys.All)
            {
                var c = await _journeyService.CreateOrUpdateAsync(journey, req);
            }

            var orgs = new List<Organisation>();
            foreach (var org in SampleOrgs.All)
            {
                orgs.Add(org);
                await _orgRepository.CreateOrUpdateItemAsync(org, req);
            }

            foreach (var driver in SampleDrivers.All)
            {
                driver.Organisation = SampleOrgs.Organisation1;
                
                var t = await _driverRepo.CreateOrUpdateItemAsync(driver, req);
            }

            var vehicles = new List<Vehicle>();
            foreach (var vehicle in SampleVehicles.All)
            {
                vehicles.Add(vehicle);
                await _vehicleRepo.CreateOrUpdateItemAsync(vehicle, req);
            }

            foreach (var journey in SampleJourneys.All)
            {
                await _journeyRepository.CreateOrUpdateItemAsync(journey, req);
            }

            var config = await _configRepo.GetItemAsync(Config.ConfigId);
            
            return req.CreateResponse(HttpStatusCode.Created);
        }

        public async Task SetupBaseData(HttpRequestMessage req)
        {
            var config = await _configRepo.GetItemAsync(Config.ConfigId);
            if (config != null && config.BaseDataSetup)
            {
                return;
            }
            try
            {
                await _userService.GetAllUsers(req);    //This will save all users into the document DB, this may be slow if there are many users!
            }
            catch (Exception exp)
            {
                throw new Exception("Unable to save all users: " + exp.Message + exp.StackTrace);
            }

            await _metadataRepository.CreateOrUpdateItemAsync(BaseMetadata.metaData, req);
            await _eventReminderRepo.CreateOrUpdateItemAsync(BaseEventReminder.EventReminder, req);
            

            await _configRepo.CreateOrUpdateItemAsync(new Config()
            {
                id = Config.ConfigId,
                BaseDataSetup = true,
                JourneyCounter = 1,
                JourneyApprovalsEnabled = false // Note - The approvals feature was implemented before client decided it wasn't needed. Can turn on again here if they change their mind again!
            }, req);
        }

        private async Task UpdateUserRoles(HttpRequestMessage req)
        {
            var mikeDriverAdmin = SampleUsers.DriverAdmin;

            var user = await _userDBRepository.GetItemAsync(mikeDriverAdmin.id);
            user.organisationId = mikeDriverAdmin.organisationId;
            user.organisationName = mikeDriverAdmin.organisationName;
            user.appRole = mikeDriverAdmin.appRole;
            user.appRoleDisplayName = mikeDriverAdmin.appRoleDisplayName;
            user.driverId = mikeDriverAdmin.driverId;
            await _userDBRepository.UpdateItemAsync(user.id, user, _userDigestService, req);
        }

        private async Task<SavedFile> AddSampleFile(HttpRequestMessage req)
        {
            var bytes = Convert.FromBase64String(SampleSavedFile.fileContents);
            Stream stream = new MemoryStream(bytes);

            return await _fileService.AddFileAsync(stream, SampleSavedFile.documentId, SampleSavedFile.fileName, "", req);
        }
    }
}