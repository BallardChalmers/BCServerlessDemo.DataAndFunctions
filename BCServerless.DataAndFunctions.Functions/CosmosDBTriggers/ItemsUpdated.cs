using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Service.Synchronisers;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.CosmosDBTriggers
{
    public class ItemsUpdated
    {
        [FunctionName("ItemsUpdated")]
        public static async Task Run(
        [CosmosDBTrigger("BCServerlessDemoDB", "Items", ConnectionStringSetting = "CosmosConnectionString", CreateLeaseCollectionIfNotExists = true)] JArray documents,
        TraceWriter log,
        [Inject(typeof(IDriverSynchroniser))]IDriverSynchroniser driverSynchroniser,
        [Inject(typeof(IOrganisationSynchroniser))]IOrganisationSynchroniser organisationSynchroniser)
        {
            log.Info("Documents modified " + documents.Count);

            foreach (var doc in documents.Children<JObject>())
            {
                try
                {
                    var type = (string)doc.Property("type").Value;

                    if (type == typeof(Driver).Name)
                    {
                        var driver = doc.ToObject<Driver>();
                        await driverSynchroniser.SyncAsync(driver);
                    }

                    if (type == typeof(Organisation).Name)
                    {
                        var organisation = doc.ToObject<Organisation>();
                        await organisationSynchroniser.SyncAsync(organisation);
                    }
                }
                catch(Exception ex)
                {
                    log.Error($"Unable to update item: {doc.ToString()}", ex);
                }
            }    
        }
    }
}
