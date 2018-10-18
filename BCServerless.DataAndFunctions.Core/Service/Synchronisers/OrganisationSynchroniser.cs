using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers
{
    public interface IOrganisationSynchroniser
    {
        Task SyncAsync(Organisation organisation);
    }

    public class OrganisationSynchroniser : IOrganisationSynchroniser
    {
        private readonly IDocumentDBRepository<Driver> _driverRepo;
        private readonly IDocumentDBRepository<Journey> _journeyRepo;

        public OrganisationSynchroniser(IDocumentDBRepository<Driver> driverRepo,
            IDocumentDBRepository<Journey> journeyRepo)
        {
            _driverRepo = driverRepo;
            _journeyRepo = journeyRepo;
        }

        public async Task SyncAsync(Organisation organisation) 
        {
            // Update drivers
            var drivers = await _driverRepo.GetItemsAsync(c => c.Organisation.id == organisation.id);
            foreach(var driver in drivers)
            {
                driver.Organisation = organisation;
                await _driverRepo.UpdateItemAsync(driver.id, driver, null);
            }

            // Update journeys
            var journeys = await _journeyRepo.GetItemsAsync(c => c./*Driver.Organisation.*/id == organisation.id);
            foreach (var journey in journeys)
            {
                //  journey.Driver.Organisation = organisation;
                await _journeyRepo.UpdateItemAsync(journey.id, journey, null);
            }
        }

    }
}
