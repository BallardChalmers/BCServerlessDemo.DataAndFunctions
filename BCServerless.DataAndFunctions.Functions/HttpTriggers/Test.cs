using BCServerlessDemo.DataAndFunctions.Core.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers
{
    public class Test : BaseHttpTrigger
    {
        [FunctionName("Test")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, methods: new string[] { "GET" })]HttpRequestMessage req,
            TraceWriter log)
        {
            List<string> filterNames = req.GetQueryNameValuePairs().Where(w => w.Key == "f").Select(q => q.Value).ToList();
            var values = req.GetQueryNameValuePairs().Where(w => w.Key == "v").Select(q => q.Value).ToList();

            var filters = new List<GridQueryFilter>();
            for(int i = 0; i < filterNames.Count; i++)
            {
                filters.Add(new GridQueryFilter() { column = filterNames[i], value = values.ElementAtOrDefault(i) });
            }


            return req.CreateResponse(HttpStatusCode.OK, "Functions, ready to go!");

           // AddCORSHeader(req, response, "GET,POST,PUT,PATCH,OPTIONS");

           // return response;
        } 
    }
}
