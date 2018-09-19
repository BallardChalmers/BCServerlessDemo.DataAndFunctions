using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IVehicleService
    {
        Task<Vehicle> GetAsync(string organisationId);
        Task<Vehicle> CreateAsync(Vehicle item, HttpRequestMessage req);
        Task<Vehicle> UpdateAsync(Vehicle item, HttpRequestMessage req);
        Task<Vehicle> CreateOrUpdateAsync(Vehicle item, HttpRequestMessage req);
    }

    public class VehicleService : IVehicleService
    {
        private readonly IDocumentDBRepository<Vehicle> _organisationRepository;
        private readonly IUserDigestService _userDigestService;

        public VehicleService(IDocumentDBRepository<Vehicle> organisationRepository,
            IUserDigestService userDigestService)
        {
            _organisationRepository = organisationRepository;
            _userDigestService = userDigestService;
        }

        public async Task<Vehicle> GetAsync(string organisationId)
        {
            return await _organisationRepository.GetItemAsync(organisationId);
        }

        public async Task<Vehicle> CreateAsync(Vehicle item, HttpRequestMessage req)
        {
            var createResult = await _organisationRepository.CreateItemAsync(item, req);

            return createResult;
        }

        public async Task<Vehicle> UpdateAsync(Vehicle item, HttpRequestMessage req)
        {
            return await _organisationRepository.UpdateItemAsync(item.id, item, req);
        }

        public async Task<Vehicle> CreateOrUpdateAsync(Vehicle item, HttpRequestMessage req)
        {
            return (item.id == null || _organisationRepository.GetItemAsync(item.id).Result == null)
                ? await CreateAsync(item, req)
            : await UpdateAsync(item, req);
        }
    }
}
