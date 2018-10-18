using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using Flurl;
using Flurl.Http;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IJourneyService 
    {
        Task<Journey> Get(string id);
        Task<Journey> CreateAsync(Journey item, HttpRequestMessage req);
        Task<Journey> UpdateAsync(Journey item, HttpRequestMessage req);
        Task<Journey> CreateOrUpdateAsync(Journey item, HttpRequestMessage req);
        Task<Journey> DeleteAsync(Journey Journey, HttpRequestMessage req);
    }

    public class JourneyService : IJourneyService
    {
        private readonly IDocumentDBRepository<Journey> _JourneyRepository;
        private readonly IUserDigestService _userDigestService;
        private readonly IDocumentDBRepository<Config> _configRepository;
        private readonly IApplicationConfig _applicationConfig;

        public JourneyService(IDocumentDBRepository<Journey> JourneyRepository, 
            IUserDigestService userDigestService, 
            IDocumentDBRepository<Config> configRepository,
            IApplicationConfig applicationConfig)    
        {
            _JourneyRepository = JourneyRepository;
            _userDigestService = userDigestService;
            _configRepository = configRepository;
            _applicationConfig = applicationConfig;
        }      

        public async Task<Journey> Get(string id)
        {
            var items = await _JourneyRepository.GetItemsAsync(c => c.id == id && c.deleted == false);
            return items.Count() == 1 ? items.First() : null;
        }

        public async Task<Journey> CreateAsync(Journey item, HttpRequestMessage req)
        {
            var config = await _configRepository.GetItemAsync(Config.ConfigId);
            await _configRepository.UpdateItemAsync(config.id, config, req);

            if (item.DropoffAddress == null && (item.DropoffLatitude != null && item.DropoffLongitude != null))
            {
                var result = await "http://dev.virtualearth.net/REST/v1/Locations"
                    .AppendPathSegment(item.DropoffLatitude.ToString() + "," + item.DropoffLongitude.ToString())
                    .SetQueryParams(new { o = "json" })
                    .SetQueryParams(new { key = _applicationConfig.BingMapsAPI })
                    .GetJsonAsync<Location>();
                item.DropoffAddress = new Address() {
                    Name = result.resourceSets[0].resources[0].address.Name,
                    AddressLine1 = result.resourceSets[0].resources[0].address.AddressLine1,
                    AddressLine2 = result.resourceSets[0].resources[0].address.AddressLine2,
                    TownCity = result.resourceSets[0].resources[0].address.TownCity,
                    County = result.resourceSets[0].resources[0].address.County,
                    Postcode = result.resourceSets[0].resources[0].address.Postcode
                };
                // http://dev.virtualearth.net/REST/v1/Locations/47.64054,-122.12934?o=xml&key=BingMapsKey

            }

            return await _JourneyRepository.CreateItemAsync(item, req);        
        }

        public async Task<Journey> UpdateAsync(Journey item, HttpRequestMessage req)
        {
            var config = await _configRepository.GetItemAsync(Config.ConfigId);

            if (item.DropoffAddress != null && item.DropoffAddress.Name != null)
            {
                var result = await "http://dev.virtualearth.net/REST/v1/Locations"
                    .AppendPathSegment(item.DropoffAddress.Name)
                    .SetQueryParams(new { o = "json" })
                    .SetQueryParams(new { key = _applicationConfig.BingMapsAPI })
                    .GetJsonAsync<Location>();
                item.DropoffAddress = new Address()
                {
                    Name = result.resourceSets[0].resources[0].name,
                    AddressLine1 = result.resourceSets[0].resources[0].address.AddressLine1,
                    AddressLine2 = result.resourceSets[0].resources[0].address.AddressLine2,
                    TownCity = result.resourceSets[0].resources[0].address.TownCity,
                    County = result.resourceSets[0].resources[0].address.County,
                    Postcode = result.resourceSets[0].resources[0].address.Postcode
                };
                item.DropoffLatitude = result.resourceSets[0].resources[0].point.coordinates[0];
                item.DropoffLongitude = result.resourceSets[0].resources[0].point.coordinates[1];
                // http://dev.virtualearth.net/REST/v1/Locations/locationQuery?includeNeighborhood=includeNeighborhood&maxResults=maxResults&include=includeValue&key=BingMapsKey
            }
            else if ((item.DropoffAddress == null || item.DropoffAddress.Name == null) && item.DropoffLatitude != 0 && item.DropoffLongitude != 0)
            {
                var result = await "http://dev.virtualearth.net/REST/v1/Locations"
                    .AppendPathSegment(item.DropoffLatitude.ToString() + "," + item.DropoffLongitude.ToString())
                    .SetQueryParams(new { o = "json" })
                    .SetQueryParams(new { key = _applicationConfig.BingMapsAPI })
                    .GetJsonAsync<Location>();
                item.DropoffAddress = new Address()
                {
                    Name = result.resourceSets[0].resources[0].name,
                    AddressLine1 = result.resourceSets[0].resources[0].address.AddressLine1,
                    AddressLine2 = result.resourceSets[0].resources[0].address.AddressLine2,
                    TownCity = result.resourceSets[0].resources[0].address.TownCity,
                    County = result.resourceSets[0].resources[0].address.County,
                    Postcode = result.resourceSets[0].resources[0].address.Postcode
                };
                // http://dev.virtualearth.net/REST/v1/Locations/47.64054,-122.12934?o=xml&key=BingMapsKey

            }

            if (item.PickupAddress != null && item.PickupAddress.Name != null)
            {
                var result = await "http://dev.virtualearth.net/REST/v1/Locations"
                    .AppendPathSegment(item.PickupAddress.Name)
                    .SetQueryParams(new { o = "json" })
                    .SetQueryParams(new { key = _applicationConfig.BingMapsAPI })
                    .GetJsonAsync<Location>();
                item.PickupAddress = new Address()
                {
                    Name = result.resourceSets[0].resources[0].name,
                    AddressLine1 = result.resourceSets[0].resources[0].address.AddressLine1,
                    AddressLine2 = result.resourceSets[0].resources[0].address.AddressLine2,
                    TownCity = result.resourceSets[0].resources[0].address.TownCity,
                    County = result.resourceSets[0].resources[0].address.County,
                    Postcode = result.resourceSets[0].resources[0].address.Postcode
                };
                item.PickupLatitude = result.resourceSets[0].resources[0].point.coordinates[0];
                item.PickupLongitude = result.resourceSets[0].resources[0].point.coordinates[1];
                // http://dev.virtualearth.net/REST/v1/Locations/locationQuery?includeNeighborhood=includeNeighborhood&maxResults=maxResults&include=includeValue&key=BingMapsKey
            }
            else if ((item.PickupAddress == null || item.PickupAddress.Name == null) && item.PickupLatitude != 0 && item.PickupLongitude != 0)
            {
                var result = await "http://dev.virtualearth.net/REST/v1/Locations"
                    .AppendPathSegment(item.PickupLatitude.ToString() + "," + item.PickupLongitude.ToString())
                    .SetQueryParams(new { o = "json" })
                    .SetQueryParams(new { key = _applicationConfig.BingMapsAPI })
                    .GetJsonAsync<Location>();
                item.PickupAddress = new Address()
                {
                    Name = result.resourceSets[0].resources[0].name,
                    AddressLine1 = result.resourceSets[0].resources[0].address.AddressLine1,
                    AddressLine2 = result.resourceSets[0].resources[0].address.AddressLine2,
                    TownCity = result.resourceSets[0].resources[0].address.TownCity,
                    County = result.resourceSets[0].resources[0].address.County,
                    Postcode = result.resourceSets[0].resources[0].address.Postcode
                };
                // http://dev.virtualearth.net/REST/v1/Locations/47.64054,-122.12934?o=xml&key=BingMapsKey

            }

            return await _JourneyRepository.UpdateItemAsync(item.id, item, req);
        }

        public async Task<Journey> CreateOrUpdateAsync(Journey item, HttpRequestMessage req)
        {
            var dbResult = (item.id == null || _JourneyRepository.GetItemAsync(item.id).Result == null)
                ? await CreateAsync(item, req)
                : await UpdateAsync(item, req);

            return dbResult;
        }

        public async Task<Journey> DeleteAsync(Journey Journey, HttpRequestMessage req)
        {
            Journey.deleted = true;
            return await _JourneyRepository.DeleteItemAsync(Journey.id, Journey, req);
            
            return Journey;
        }

        private async Task<Journey> UpdateApprovalRequest(Journey item, HttpRequestMessage req)
        {
            var JourneyBefore = await _JourneyRepository.GetItemAsync(item.id);
            await _JourneyRepository.UpdateItemAsync(JourneyBefore.id, JourneyBefore, req);
            return item;
        }
    }
}
