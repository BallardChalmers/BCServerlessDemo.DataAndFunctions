using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
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

        public JourneyService(IDocumentDBRepository<Journey> JourneyRepository, 
            IUserDigestService userDigestService, 
            IDocumentDBRepository<Config> configRepository)    
        {
            _JourneyRepository = JourneyRepository;
            _userDigestService = userDigestService;
            _configRepository = configRepository;
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

            return await _JourneyRepository.CreateItemAsync(item, req);        
        }

        public async Task<Journey> UpdateAsync(Journey item, HttpRequestMessage req)
        {
            var config = await _configRepository.GetItemAsync(Config.ConfigId);
            
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
