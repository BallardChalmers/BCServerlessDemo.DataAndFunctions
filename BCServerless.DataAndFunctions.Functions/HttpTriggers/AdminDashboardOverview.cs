using BCServerlessDemo.DataAndFunctions.Functions.Api;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers
{
    public class AdminDashboardOverview : BaseHttpTrigger
    {
        [FunctionName("AdminDashboardOverview")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: new string[] { "GET", "OPTIONS" })]HttpRequestMessage req,
            TraceWriter log,
            [Inject(typeof(IAdminDashboardOverviewApi))]IAdminDashboardOverviewApi adminOverviewApi)
        {
  
            var httpMethods = new Dictionary<string, Func<HttpRequestMessage, TraceWriter, Task<HttpResponseMessage>>>
            {
                { "GET", async (r, l) => await adminOverviewApi.Get(r, l) }
            };

            var response = httpMethods.ContainsKey(req.Method.Method) ? await httpMethods[req.Method.Method](req, log) 
                : req.CreateResponse(req.Method.Method == "OPTIONS" ? HttpStatusCode.OK : HttpStatusCode.NotFound);

            AddCORSHeader(req, response, $"GET, OPTIONS");

            return response;
        } 
    }
}
