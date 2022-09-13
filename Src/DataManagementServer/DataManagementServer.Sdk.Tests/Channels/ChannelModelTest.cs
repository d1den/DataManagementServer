using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using DataManagementServer.Sdk.Channels;

namespace DataManagementServer.Sdk.Tests.Channels
{
    [TestClass]
    public class ChannelModelTest
    {
        [TestMethod]
        public void SetGrouId_FieldsContainGrouId()
        {
            // Arrange
            var arrangeGroupId = Guid.NewGuid();

            var model = new ChannelModel()
            {
                GroupId = arrangeGroupId
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.GroupId, out Guid groupId);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(groupId, arrangeGroupId);
        }

        [TestMethod]
        public void GetGroupId_FieldsContainGroupId_GetGroupId()
        {
            // Arrange
            var arrangeGroupId = Guid.NewGuid();

            var model = new ChannelModel()
            {
                [ChannelScheme.GroupId] = arrangeGroupId
            };

            // Act
            var groupId = model.GroupId;

            // Assert
            Assert.IsTrue(groupId.HasValue);
            Assert.AreEqual(groupId.Value, arrangeGroupId);
        }

        [TestMethod]
        public void GetGroupId_FieldsDontContainGroupId_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var groupId = model.GroupId;

            // Assert
            Assert.IsFalse(groupId.HasValue);
        }

        [TestMethod]
        public void SetName_FieldsContainName()
        {
            // Arrange
            var arrangeName = "Test";

            var model = new ChannelModel()
            {
                Name = arrangeName
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.Name, out string name);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(name, arrangeName);
        }

        [TestMethod]
        public void GetName_FieldsContainName_GetName()
        {
            // Arrange
            var arrangeName = "Test";

            var model = new ChannelModel()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            var name = model.Name;

            // Assert
            Assert.AreEqual(name, arrangeName);
        }

        [TestMethod]
        public void GetName_FieldsDontContainName_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var name = model.Name;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(name));
        }

        [TestMethod]
        public void SetDescription_FieldsContainDescription()
        {
            // Arrange
            var arrangeDescription = "Test";

            var model = new ChannelModel()
            {
                Description = arrangeDescription
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.Description, out string Description);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(Description, arrangeDescription);
        }

        [TestMethod]
        public void GetDescription_FieldsContainDescription_GetDescription()
        {
            // Arrange
            var arrangeDescription = "Test";

            var model = new ChannelModel()
            {
                [ChannelScheme.Description] = arrangeDescription
            };

            // Act
            var Description = model.Description;

            // Assert
            Assert.AreEqual(Description, arrangeDescription);
        }

        [TestMethod]
        public void GetDescription_FieldsDontContainDescription_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var Description = model.Description;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(Description));
        }

        [TestMethod]
        public void SetValueType_FieldsContainValueType()
        {
            // Arrange
            var arrangeValueType = TypeCode.Int32;

            var model = new ChannelModel()
            {
                ValueType = arrangeValueType
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.ValueType, out TypeCode ValueType);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(ValueType, arrangeValueType);
        }

        [TestMethod]
        public void GetValueType_FieldsContainValueType_GetValueType()
        {
            // Arrange
            var arrangeValueType = TypeCode.Int32;

            var model = new ChannelModel()
            {
                [ChannelScheme.ValueType] = arrangeValueType
            };

            // Act
            var ValueType = model.ValueType;

            // Assert
            Assert.AreEqual(ValueType, arrangeValueType);
        }

        [TestMethod]
        public void GetValueType_FieldsDontContainValueType_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var ValueType = model.ValueType;

            // Assert
            Assert.IsFalse(ValueType.HasValue);
        }

        [TestMethod]
        public void SetValue_FieldsContainValue()
        {
            // Arrange
            var arrangeValue = DateTime.Now;

            var model = new ChannelModel()
            {
                Value = arrangeValue
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.Value, out DateTime Value);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(Value, arrangeValue);
        }

        [TestMethod]
        public void GetValue_FieldsContainValue_GetValue()
        {
            // Arrange
            var arrangeValue = DateTime.Now;

            var model = new ChannelModel()
            {
                [ChannelScheme.Value] = arrangeValue
            };

            // Act
            var Value = model.Value;

            // Assert
            Assert.AreEqual(Value, arrangeValue);
        }

        [TestMethod]
        public void GetValue_FieldsDontContainValue_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var Value = model.Value;

            // Assert
            Assert.AreEqual(Value, null);
        }

        [TestMethod]
        public void SetUpdateOn_FieldsContainUpdateOn()
        {
            // Arrange
            var arrangeUpdateOn = DateTime.Now;

            var model = new ChannelModel()
            {
                UpdateOn = arrangeUpdateOn
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.UpdateOn, out DateTime UpdateOnType);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(UpdateOnType, arrangeUpdateOn);
        }

        [TestMethod]
        public void GetUpdateOn_FieldsContainUpdateOn_GetUpdateOn()
        {
            // Arrange
            var arrangeUpdateOn = DateTime.Now;

            var model = new ChannelModel()
            {
                [ChannelScheme.UpdateOn] = arrangeUpdateOn
            };

            // Act
            var UpdateOn = model.UpdateOn;

            // Assert
            Assert.AreEqual(UpdateOn, arrangeUpdateOn);
        }

        [TestMethod]
        public void GetUpdateOn_FieldsDontContainUpdateOn_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var UpdateOn = model.UpdateOn;

            // Assert
            Assert.IsFalse(UpdateOn.HasValue);
        }

        [TestMethod]
        public void SetStatus_FieldsContainStatus()
        {
            // Arrange
            var arrangeStatus = ChannelStatus.Bad;

            var model = new ChannelModel()
            {
                Status = arrangeStatus
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(ChannelScheme.Status, out ChannelStatus StatusType);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(StatusType, arrangeStatus);
        }

        [TestMethod]
        public void GetStatus_FieldsContainStatus_GetStatus()
        {
            // Arrange
            var arrangeStatus = ChannelStatus.Bad;

            var model = new ChannelModel()
            {
                [ChannelScheme.Status] = arrangeStatus
            };

            // Act
            var Status = model.Status;

            // Assert
            Assert.AreEqual(Status, arrangeStatus);
        }

        [TestMethod]
        public void GetStatus_FieldsDontContainStatus_GetNull()
        {
            // Arrange
            var model = new ChannelModel();

            // Act
            var Status = model.Status;

            // Assert
            Assert.IsFalse(Status.HasValue);
        }

        [TestMethod]
        public void Serialize_FieldsContainName_GetCorrectJson()
        {
            // Arrange
            var arrangeName = "Test";
            var model = new ChannelModel();
            model.Name = arrangeName;

            // Act
            var json = JsonConvert.SerializeObject(model);

            // Assert
            Assert.IsFalse(json.Contains(ChannelScheme.GroupId));
            Assert.IsTrue(json.Contains("Fields"));
            Assert.IsTrue(json.Contains(ChannelScheme.Name));
            Assert.IsTrue(json.Contains(arrangeName));
        }
    }
}
