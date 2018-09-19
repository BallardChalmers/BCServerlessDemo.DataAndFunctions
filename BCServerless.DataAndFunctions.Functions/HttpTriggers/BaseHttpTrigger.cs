using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers
{
    public abstract class BaseHttpTrigger
    {
        public static void AddCORSHeader(HttpRequestMessage request, HttpResponseMessage response, string methods)
        {
            if (request.Headers.Contains("Origin"))
            {
                var origin = request.Headers.GetValues("Origin").FirstOrDefault();

                response.Headers.Add("Access-Control-Allow-Credentials", "true");
                response.Headers.Add("Access-Control-Allow-Origin", origin);
                response.Headers.Add("Access-Control-Allow-Methods", methods);
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Cache-Control,Pragma,Authorization,UserId"); 
                response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            }
        }

        public static void AddCORSHeader(HttpResponseMessage response, string methods)
        {
            response.Headers.Add("Access-Control-Allow-Credentials", "true");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
            response.Headers.Add("Access-Control-Allow-Methods", methods);
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type,Cache-Control,Pragma,Authorization,UserId");
            response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
        }

    }
}
