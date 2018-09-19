using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public interface IDriverService
    {
        Task<Driver> CreateAsync(Driver Driver, HttpRequestMessage req);
        Task<Driver> UpdateAsync(Driver Driver, HttpRequestMessage req);
        Task<Driver> DeleteAsync(Driver Driver, HttpRequestMessage req);
    }

    public class DriverService : IDriverService
    {
        private readonly IDocumentDBRepository<Driver> _DriverRepository;
        private readonly IUserService _userService;
        private readonly IUserDigestService _userDigestService;

        public DriverService(IDocumentDBRepository<Driver> DriverRepository,
            IUserService userService,
            IUserDigestService userDigestService)
        {
            _DriverRepository = DriverRepository;
            _userService = userService;
            _userDigestService = userDigestService;
        }
        
        public async Task<Driver> CreateAsync(Driver Driver, HttpRequestMessage req)
        {
            Driver = await _DriverRepository.CreateItemAsync(Driver, req);
            return Driver;
        }

        public async Task<Driver> UpdateAsync(Driver Driver, HttpRequestMessage req)
        {
            var DriverBefore = await _DriverRepository.GetItemAsync(Driver.id);

            
            Driver = await _DriverRepository.UpdateItemAsync(Driver.id, Driver, req);
            return Driver;
            
        }

        public async Task<Driver> DeleteAsync(Driver Driver, HttpRequestMessage req)
        {
            var DriverBefore = await _DriverRepository.GetItemAsync(Driver.id);


            Driver = await _DriverRepository.DeleteItemAsync(Driver.id, Driver, req);
            return Driver;

        }

    }
}
