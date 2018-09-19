using BCServerlessDemo.DataAndFunctions.Functions.Api;
using BCServerlessDemo.DataAndFunctions.Functions.HttpTriggers;
using BCServerlessDemo.DataAndFunctions.Functions.Startup;
using NUnit.Framework;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BCServerlessDemo.DataAndFunctions.Tests.Startup
{
    
    [TestFixture]
    public class ContainerRegistrarTests
    {
        Container container;

        [Test]
        public void AllServicesRegistered()
        {
            container = ContainerRegistrar.GetContainer();            

            var apiTypes = GetApiTypes();

            foreach (Type apiType in apiTypes)
            {
                try
                {
                    var controller = container.GetInstance(apiType);
                }
                catch (Exception ex)
                {
                    Assert.Fail(ex.ToString());
                }
            }

            // Stops the test runner crapping out in VSTS
            Thread.Sleep(1000 * 5);
        }

        private List<Type> GetApiTypes()
        {
            var functionsAssembly = typeof(BaseHttpTrigger).Assembly;
            return functionsAssembly.GetTypes().Where(type => type.IsInterface && type.GetInterfaces().Contains((typeof(IHttpApi)))).ToList();
        }       
    }
    
    
}
