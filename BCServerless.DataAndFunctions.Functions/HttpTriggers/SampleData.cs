using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BCServerlessDemo.DataAndFunctions.Functions.Api;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers
{
    public class SampleData : BaseHttpTrigger
    {
        [FunctionName("SampleData")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log,
            [Inject(typeof(ISampleDataApi))]ISampleDataApi _sampleApi)
        {

            var httpMethods = new Dictionary<string, Func<HttpRequestMessage, TraceWriter, Task<HttpResponseMessage>>>
            {  
                { "POST", async (r, l) => await _sampleApi.Post(r, l) } 
            };

            var response = httpMethods.ContainsKey(req.Method.Method) ? await httpMethods[req.Method.Method](req, log) 
                : req.CreateResponse(req.Method.Method == "OPTIONS" ? HttpStatusCode.OK : HttpStatusCode.NotFound);

            AddCORSHeader(req, response, $"POST, OPTIONS");

            return req.CreateResponse(HttpStatusCode.Created);
        }
    }
}
