using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioPlayer.Controllers;
using RadioPlayer.Models;
using System.Collections.Generic;

namespace RadioPlayerTests.ControllersTests
{
    [TestClass]
    public class StationsControllerTest
    {
        [TestMethod]
        public void SaveToFileTest()
        {
            List<Station> stations = new List<Station>()
            {
                new Station("station1", "address1"),
                new Station("station2", "address2"),
                new Station("station3", "address3")
            };
            StationsController stationsController = new StationsController();

            bool result = stationsController.SaveToFile(stations);

            Assert.IsTrue(result);
        }
    }
}
