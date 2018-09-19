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
    public class Journeys : BaseHttpTrigger
    {
        [FunctionName("Journeys")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: new string[] { "GET", "DELETE", "POST", "PUT", "OPTIONS" })]HttpRequestMessage req,
            TraceWriter log,
            [Inject(typeof(IJourneysApi))]IJourneysApi journeysApi)
        {
  
            var httpMethods = new Dictionary<string, Func<HttpRequestMessage, TraceWriter, Task<HttpResponseMessage>>>
            {
                { "GET", async (r, l) => await journeysApi.Get(r, l) },
                { "POST", async (r, l) => await journeysApi.Post(r, l) },
                { "PUT", async (r, l) => await journeysApi.Put(r, l) },
                { "DELETE", async (r, l) => await journeysApi.Delete(r, l) },
            };

            var response = httpMethods.ContainsKey(req.Method.Method) ? await httpMethods[req.Method.Method](req, log)
                : req.CreateResponse(req.Method.Method == "OPTIONS" ? HttpStatusCode.OK : HttpStatusCode.NotFound);

            AddCORSHeader(req, response, "GET, DELETE, POST, PUT, OPTIONS");

            return response;
        } 
    }
}
