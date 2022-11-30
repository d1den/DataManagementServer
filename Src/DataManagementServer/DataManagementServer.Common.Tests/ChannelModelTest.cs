using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;

namespace DataManagementServer.Common.Tests
{

    [TestClass]
    public class ChannelModelTest
    {
        [TestMethod]
        public void Serialize()
        {
            //Arrange
            var model = new ChannelModel()
            {
                GroupId = Guid.NewGuid(),
                Name = "Test",
                Description = "Json test",
                ValueType = TypeCode.Int64,
                Value = 12,
                UpdateOn = DateTime.Now,
                Status = ChannelStatus.Good
            };

            //Act
            var json = JsonConvert.SerializeObject(model);

            //Assert
            Assert.IsTrue(json.Contains(ChannelScheme.GroupId));
            Assert.IsTrue(json.Contains(ChannelScheme.Name));
            Assert.IsTrue(json.Contains(ChannelScheme.Description));
            Assert.IsTrue(json.Contains(ChannelScheme.ValueType));
            Assert.IsTrue(json.Contains(ChannelScheme.Value));
            Assert.IsTrue(json.Contains(ChannelScheme.UpdateOn));
            Assert.IsTrue(json.Contains(ChannelScheme.Status));
        }

        [TestMethod]
        public void Deserialize()
        {
            // Так должно быть, но пока не работает
            // + надо проверять на парсере asp.net, чтобы был единый парсер везде
            //Arrange
            var model = new ChannelModel()
            {
                GroupId = Guid.NewGuid(),
                Name = "Test",
                Description = "Json test",
                ValueType = TypeCode.Int64,
                Value = 12,
                UpdateOn = DateTime.Now,
                Status = ChannelStatus.Good
            };

            //Act
            var json = JsonConvert.SerializeObject(model);
            var newModel = JsonConvert.DeserializeObject<ChannelModel>(json);

            //Assert
            Assert.AreEqual(model.GroupId, newModel.GroupId);
            Assert.AreEqual(model.Name, newModel.Name);
            Assert.AreEqual(model.Description, newModel.Description);
            Assert.AreEqual(model.ValueType, newModel.ValueType);
            Assert.AreEqual(model.Value, newModel.Value);
            Assert.AreEqual(model.UpdateOn, newModel.UpdateOn);
            Assert.AreEqual(model.Status, newModel.Status);
        }
    }
}
