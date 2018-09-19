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
using System.Security.Claims;
using System.Linq;

namespace BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers
{
    public class Startup : BaseHttpTrigger
    {
        /// <summary>
        /// This function should be called once when the application starts.
        /// </summary>
        [FunctionName("Startup")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: new string[] { "GET", "OPTIONS" })]HttpRequestMessage req,
            TraceWriter log,
            [Inject(typeof(IStartupApi))]IStartupApi startupApi)
        {
            log.Info("Listing the claims...");
            log.Info("Claims count: " + ClaimsPrincipal.Current.Claims.Count().ToString());
            foreach (var claim in ClaimsPrincipal.Current.Claims)
            {
                log.Info("Claim type: " + claim.Type + ", Claim value: " + claim.Value);
            }

            var httpMethods = new Dictionary<string, Func<HttpRequestMessage, TraceWriter, Task<HttpResponseMessage>>>
            {
                { "GET", async (r, l) => {
                    try {
                    return await startupApi.Get(r, l);
                    }
                    catch (Exception exp)
                    {
                        log.Error("There was an error setting up the startup details: " + exp.Message);
                        return req.CreateResponse(HttpStatusCode.BadRequest);
                    }
                } }
            };

            var response = httpMethods.ContainsKey(req.Method.Method) ? await httpMethods[req.Method.Method](req, log) :
                req.CreateResponse(req.Method.Method == "OPTIONS" ? HttpStatusCode.OK : HttpStatusCode.NotFound);

            AddCORSHeader(req, response, $"GET, OPTIONS");

            return response;
        } 
    }
}
