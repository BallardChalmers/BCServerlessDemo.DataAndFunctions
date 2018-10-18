using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IOrganisationService
    {
        Task<Organisation> GetAsync(string organisationId);
        Task<Organisation> CreateAsync(Organisation item, HttpRequestMessage req);
        Task<Organisation> UpdateAsync(Organisation item, HttpRequestMessage req);
        Task<Organisation> CreateOrUpdateAsync(Organisation item, HttpRequestMessage req);
        Task<Organisation> DeleteAsync(Organisation item, HttpRequestMessage req);
    }

    public class OrganisationService : IOrganisationService
    {
        private readonly IDocumentDBRepository<Organisation> _organisationRepository;
        private readonly IUserDigestService _userDigestService;

        public OrganisationService(IDocumentDBRepository<Organisation> organisationRepository,
            IUserDigestService userDigestService)
        {
            _organisationRepository = organisationRepository;
            _userDigestService = userDigestService;
        }

        public async Task<Organisation> GetAsync(string organisationId)
        {
            return await _organisationRepository.GetItemAsync(organisationId);
        }
        
        public async Task<Organisation> CreateAsync(Organisation item, HttpRequestMessage req)
        {
            var createResult = await _organisationRepository.CreateItemAsync(item, req);

            return createResult;
        }

        public async Task<Organisation> UpdateAsync(Organisation item, HttpRequestMessage req)
        {
            return await _organisationRepository.UpdateItemAsync(item.id, item, req);
        }

        public async Task<Organisation> CreateOrUpdateAsync(Organisation item, HttpRequestMessage req)
        {
            return (item.id == null || _organisationRepository.GetItemAsync(item.id).Result == null)
                ? await CreateAsync(item, req)
            : await UpdateAsync(item, req);
        }

        public async Task<Organisation> DeleteAsync(Organisation item, HttpRequestMessage req)
        {
            var OrganisationBefore = await _organisationRepository.GetItemAsync(item.id);
            
            item = await _organisationRepository.DeleteItemAsync(item.id, item, req);
            return item;

        }
    }
}