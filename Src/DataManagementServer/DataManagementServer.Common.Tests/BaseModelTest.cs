using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace DataManagementServer.Common.Tests
{
    [TestClass]
    public class BaseModelTest
    {
        [TestMethod]
        public void CastToModel_CloneableFields()
        {
            //Arrange
            var pollingPeriod = 500;
            var baseModel = new BaseModel(Guid.NewGuid())
            {
                [DeviceScheme.PollingPeriod] = pollingPeriod
            };

            //Act
            var deviceModel = baseModel.CastToModel<DeviceModel>();
            //Assert
            Assert.IsInstanceOfType(deviceModel, typeof(DeviceModel));
            Assert.AreEqual(baseModel.Id, deviceModel.Id);
            Assert.IsTrue(deviceModel.PollingPeriod.HasValue);
            Assert.AreEqual(pollingPeriod, deviceModel.PollingPeriod.Value);
        }

        [TestMethod]
        public void CastToModel_NotCloneableFields_Exception()
        {
            //Arrange
            var pollingPeriod = 500;
            var baseModel = new BaseModel(Guid.NewGuid())
            {
                [DeviceScheme.PollingPeriod] = pollingPeriod,
                ["test"] = new DeviceModel()
            };

            //Act
            //Assert
            Assert.ThrowsException<Exception>(() => baseModel.CastToModel<DeviceModel>());
        }

        [TestMethod]
        public void Serialize()
        {
            //Arrange
            var pollingPeriod = 500;
            var helloWorld = "Hello, World!";
            var baseModel = new BaseModel(Guid.NewGuid())
            {
                [nameof(pollingPeriod)] = pollingPeriod,
                [nameof(helloWorld)] = helloWorld
            };

            //Act
            var json = JsonConvert.SerializeObject(baseModel);

            //Assert
            Assert.IsTrue(json.Contains(nameof(pollingPeriod)));
            Assert.IsTrue(json.Contains(nameof(helloWorld)));
        }

        [TestMethod]
        public void Deserialize()
        {
            //Arrange
            var pollingPeriod = 500;
            var helloWorld = "Hello, World!";
            var baseModel = new BaseModel(Guid.NewGuid())
            {
                [nameof(pollingPeriod)] = pollingPeriod,
                [nameof(helloWorld)] = helloWorld
            };

            //Act
            var json = JsonConvert.SerializeObject(baseModel);
            var newModel = JsonConvert.DeserializeObject<BaseModel>(json);

            //Assert
            Assert.IsTrue(newModel.Fields.ContainsKey(nameof(pollingPeriod)));
            Assert.IsTrue(newModel.Fields.ContainsKey(nameof(helloWorld)));
        }
    }
}
