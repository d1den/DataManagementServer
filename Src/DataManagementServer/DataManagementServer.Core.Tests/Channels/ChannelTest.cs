using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DataManagementServer.Core.Channels;
using DataManagementServer.Sdk.Channels;
using System.Reactive.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace DataManagementServer.Core.Tests.Channels
{
    [TestClass]
    public class ChannelTest
    {
        [TestMethod]
        public void UpdateValue_CorrectType_GetNewValue()
        {
            // Arrange
            var oldUpdateOn = DateTime.Now.AddHours(-5);
            var oldStatus = ChannelStatus.NoConnection;
            var model = new ChannelModel() 
            {
                ValueType = TypeCode.Int32, 
                Value = 12, 
                UpdateOn = oldUpdateOn, 
                Status = oldStatus
            };
            var channel = new Channel(model);
            var arrangeValue = 322;

            // Act
            channel.UpdateValue(arrangeValue);
            var newModel = channel.ToModel(true);

            // Assert
            Assert.AreEqual(newModel.Value, arrangeValue);
            Assert.AreNotEqual(newModel.UpdateOn, oldUpdateOn);
            Assert.AreNotEqual(newModel.Status, oldStatus);
        }

        [TestMethod]
        public void UpdateValue_IncorrectType_ArgumentException()
        {
            // Arrange
            var model = new ChannelModel()
            {
                ValueType = TypeCode.Int32,
                Value = 12
            };
            var channel = new Channel(model);
            var arrangeValue = 32.234;

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => channel.UpdateValue(arrangeValue));
        }

        [TestMethod]
        public void Update_ModelWithAllParams_GetWithAllParams()
        {
            // Arrange
            var channel = new Channel();

            var arrangeId = channel.Id;
            var arrangeGroupId = Guid.NewGuid();
            var arrangeName = "Test";
            var arrangeDescription = "New descr";
            var arrangeType = TypeCode.Byte;
            var arrangeValue = (byte)32;
            var arrangeStatus = ChannelStatus.NoConnection;
            var arrangeUpdatedOn = DateTime.Now.AddDays(4);
            var addField = "testFields";
            var model = new ChannelModel(arrangeId)
            {
                Name = arrangeName,
                GroupId = arrangeGroupId,
                Description = arrangeDescription,
                ValueType = arrangeType,
                Value = arrangeValue,
                Status = arrangeStatus,
                UpdateOn = arrangeUpdatedOn,
                [nameof(addField)] = addField
            };

            // Act
            channel.Update(model);
            var newModel = channel.ToModel(true);
            var addFieldValue = newModel.GetFieldValue<string>(nameof(addField));
            // Assert
            Assert.AreEqual(channel.Id, arrangeId);
            Assert.AreEqual(newModel.Name, arrangeName);
            Assert.AreEqual(newModel.GroupId, arrangeGroupId);
            Assert.AreEqual(newModel.Description, arrangeDescription);
            Assert.AreEqual(newModel.ValueType, arrangeType);
            Assert.AreEqual(newModel.Value, arrangeValue);
            Assert.AreEqual(newModel.Status, arrangeStatus);
            Assert.AreEqual(newModel.UpdateOn, arrangeUpdatedOn);
            Assert.AreEqual(addFieldValue, addField);
        }

        [TestMethod]
        public void Update_NullModel_ArgumentNullException()
        {
            // Arrange
            var channel = new Channel();
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => channel.Update(null));
        }

        [TestMethod]
        public void Update_ModelWithEmptyId_ArgumentException()
        {
            // Arrange
            var channel = new Channel();
            var model = new ChannelModel();
            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => channel.Update(model));
        }

        [TestMethod]
        public void Update_ModelWithWrongId_ArgumentException()
        {
            // Arrange
            var channel = new Channel();
            var model = new ChannelModel(Guid.NewGuid());
            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => channel.Update(model));
        }

        [TestMethod]
        public void ToModel_AllFields_GetAllFields()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeGroupId = Guid.NewGuid();
            var arrangeName = "Test name";

            var model = new ChannelModel(arrangeId) { GroupId = arrangeGroupId, Name = arrangeName };
            var addFieldName = "TestFields";
            model.Fields[addFieldName] = "new value";

            var channel = new Channel(model);

            // Act
            var newModel = channel.ToModel(true);

            // Assert
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.Description));
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.ValueType));
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.Value));
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.UpdateOn));
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.Status));
            Assert.IsTrue(newModel.Fields.ContainsKey(addFieldName));
            Assert.AreEqual(newModel.Id, arrangeId);
            Assert.AreEqual(newModel.GroupId, arrangeGroupId);
            Assert.AreEqual(newModel.Name, arrangeName);
        }

        [TestMethod]
        public void ToModel_NotAllFields_GetId()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeGroupId = Guid.NewGuid();
            var arrangeName = "Test name";

            var model = new ChannelModel(arrangeId) { GroupId = arrangeGroupId, Name = arrangeName };
            var addFieldName = "TestFields";
            model.Fields[addFieldName] = "new value";

            var channel = new Channel(model);

            // Act
            var newModel = channel.ToModel(false);

            // Assert
            Assert.AreEqual(newModel.Fields.Count, 0);
            Assert.AreEqual(newModel.Id, arrangeId);
        }

        [TestMethod]
        public void ToModel_NameAndDesctiptionAndAddFeild_GetNameAndDescriptionAndAddFeild()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeGroupId = Guid.NewGuid();
            var arrangeName = "Test name";

            var model = new ChannelModel(arrangeId) { GroupId = arrangeGroupId, Name = arrangeName };
            var addFieldName = "TestFields";
            model.Fields[addFieldName] = "new value";

            var channel = new Channel(model);
            var columns = new[] { ChannelScheme.Name, ChannelScheme.Description, addFieldName };

            // Act
            var newModel = channel.ToModel(columns);

            // Assert
            Assert.AreEqual(newModel.Fields.Count, 3);
            Assert.AreEqual(newModel.Id, arrangeId);
            Assert.AreEqual(newModel.Name, arrangeName);
            Assert.IsTrue(newModel.Fields.ContainsKey(ChannelScheme.Description));
            Assert.IsTrue(newModel.Fields.ContainsKey(addFieldName));
        }

        [TestMethod]
        public void GetValue_IntValue_TypeInt_GetIntValue()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            var value = channel.GetValue<int>();

            // Assert
            Assert.AreEqual(value, arrangeValue);
        }

        [TestMethod]
        public void GetValue_IntValue_TypeDouble_ArgumentException()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => channel.GetValue<double>());
        }

        [TestMethod]
        public void GetValue_IntValue_TypeNullableInt_GetTrueAndNullableIntValue()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            var value = channel.GetValue<int?>();

            // Assert
            Assert.AreEqual(value, arrangeValue);
        }

        [TestMethod]
        public void TryGetValue_IntValue_TypeInt_GetTrueAndIntValue()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            var result = channel.TryGetValue(out int value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, arrangeValue);
        }

        [TestMethod]
        public void TryGetValue_IntValue_TypeNullableInt_GetTrueAndNullableIntValue()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            var result = channel.TryGetValue(out int? value);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(value, arrangeValue);
        }

        [TestMethod]
        public void TryGetValue_IntValue_TypeDouble_GetFalse()
        {
            // Arrange
            var arrangeValue = 12;

            var model = new ChannelModel() { ValueType = TypeCode.Int32, Value = arrangeValue };

            var channel = new Channel(model);

            // Act
            var result = channel.TryGetValue(out double value);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Contstructor_Empty_GetWithId()
        {
            // Arrange
            // Act
            var channel = new Channel();
            var newModel = channel.ToModel(true);

            // Assert
            Assert.AreNotEqual(channel.Id, Guid.Empty);
            Assert.AreEqual(newModel.GroupId, Guid.Empty);
            Assert.AreEqual(newModel.ValueType, TypeCode.Object);
            Assert.IsNotNull(newModel.Name);
        }

        [TestMethod]
        public void Contstructor_ModelWithEmptyId_GetWithId()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var channel = new Channel(model);
            var newModel = channel.ToModel(true);

            // Assert
            Assert.AreNotEqual(channel.Id, Guid.Empty);
            Assert.AreEqual(newModel.GroupId, Guid.Empty);
            Assert.AreEqual(newModel.ValueType, TypeCode.Object);
            Assert.IsNotNull(newModel.Name);
        }

        [TestMethod]
        public void Contstructor_ModelWithAllParams_GetWithAllParams()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeGroupId = Guid.NewGuid();
            var arrangeName = "Test";
            var arrangeDescription = "New descr";
            var arrangeType = TypeCode.Byte;
            var arrangeValue = (byte)32;
            var arrangeStatus = ChannelStatus.NoConnection;
            var arrangeUpdatedOn = DateTime.Now.AddDays(4);
            var addField = "testFields";
            var model = new ChannelModel(arrangeId)
            {
                Name = arrangeName,
                GroupId = arrangeGroupId,
                Description = arrangeDescription,
                ValueType = arrangeType,
                Value = arrangeValue,
                Status = arrangeStatus,
                UpdateOn = arrangeUpdatedOn,
                [nameof(addField)] = addField
            };

            // Act
            var channel = new Channel(model);
            var newModel = channel.ToModel(true);
            var addFieldValue = newModel.GetFieldValue<string>(nameof(addField));
            // Assert
            Assert.AreEqual(channel.Id,arrangeId);
            Assert.AreEqual(newModel.Name, arrangeName);
            Assert.AreEqual(newModel.GroupId, arrangeGroupId);
            Assert.AreEqual(newModel.Description, arrangeDescription);
            Assert.AreEqual(newModel.ValueType, arrangeType);
            Assert.AreEqual(newModel.Value, arrangeValue);
            Assert.AreEqual(newModel.Status, arrangeStatus);
            Assert.AreEqual(newModel.UpdateOn, arrangeUpdatedOn);
            Assert.AreEqual(addFieldValue, addField);
        }

        [TestMethod]
        public void Contstructor_NullModel_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new Channel(null));
        }
    }
}
