using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioPlayer.Controllers;
using RadioPlayer.Models;
using System.Collections.Generic;
using System.IO;

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

            stationsController.SaveToFile(stations);

            Assert.IsTrue(File.Exists(stationsController.FileName));
        }

        [TestMethod]
        public void LoadFromFileTest()
        {
            StationsController stationsController = new StationsController();

            List<Station> stations = stationsController.LoadFromFile();

            Assert.IsTrue(stations.Count == 3);
        }
    }
}
