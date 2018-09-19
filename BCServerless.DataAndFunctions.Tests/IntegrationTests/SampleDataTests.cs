using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using System.Net.Http;

namespace BCServerlessDemo.DataAndFunctions.Tests.IntegrationTests
{
    public class SampleDataTests
    {
        [Test]
        public void FilterAuthorisedPersonByPhotoIsNullTest()
        {
            var client = new HttpClient();
            string url = Utilities.GetFunctionsUrl("SampleData");
            HttpResponseMessage response = client.PostAsJsonAsync(url, "Sample").Result;
        }
    }
}
