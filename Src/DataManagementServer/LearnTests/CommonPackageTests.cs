using DataManagementServer.Common.Models;
using Newtonsoft.Json.Linq;
using System;

namespace LearnTests
{
    [TestClass]
    public class CommonPackageTests
    {
        [TestMethod]
        public void CreateBaseDeviceModel()
        {
            BaseDeviceModel model = new();
            model.Id = Guid.NewGuid();
            model.PollingPeriod = 30;
            model.Name = "clen";
            model.Status = DeviceStatus.Runnig;

            BaseDeviceModel model1 = new();
            model.Id = Guid.NewGuid();
            model.PollingPeriod = 30;
            model.Name = "clen";
            model.Status = DeviceStatus.Runnig;
            Console.WriteLine(model);
            Console.WriteLine(model.Equals(model));
            Console.WriteLine(model.GetHashCode());
            Console.WriteLine(model1.GetHashCode());
        }

        [TestMethod]
        public void CreateFieldValueCollection()
        {
        }
    }
}