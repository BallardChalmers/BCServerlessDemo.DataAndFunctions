using BCServerlessDemo.DataAndFunctions.Core.Data;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using System.Linq;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers
{
    public interface IDriverSynchroniser
    {
        Task SyncAsync(Driver driver);
    }

    public class DriverSynchroniser : IDriverSynchroniser
    {
        private readonly IDocumentDBRepository<Journey> _journeyRepo;

        public DriverSynchroniser(IDocumentDBRepository<Journey> journeyRepo)
        {
            _journeyRepo = journeyRepo;
        }

        public async Task SyncAsync(Driver driver)
        {
            // Update drivers related to Journeys
            var journeys = await _journeyRepo.GetItemsAsync(c => c.Driver.id == driver.id);
            foreach(var journey in journeys)
            {
                journey.Driver = driver;
                await _journeyRepo.UpdateItemAsync(journey.id, journey, null);
            }
        }
    }
}
