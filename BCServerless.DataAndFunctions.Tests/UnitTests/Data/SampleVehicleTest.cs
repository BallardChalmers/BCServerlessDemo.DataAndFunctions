using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Data.Sample;

namespace BCServerlessDemo.DataAndFunctions.Tests.Data
{
    public class SampleVehicleTest
    {
        [Test()]
        public void ImportVehiclesFromFileTest()
        {
            List<Vehicle> actualVehicles = SampleVehicles.ImportVehiclesFromFile();
            Assert.IsTrue(actualVehicles.Count() > 0);
        }
    }
}
