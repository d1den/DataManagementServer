using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DataManagementServer.Sdk.Channels;

namespace DataManagementServer.Sdk.Tests.Channels
{
    [TestClass]
    public class FieldValueCollectionTest
    {
        [TestMethod]
        public void Clone_GetIndependentClone()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            var fieldsCopy = fields.Clone() as FieldValueCollection;
            fields[ChannelScheme.Name] = "new name";

            // Assert
            Assert.AreEqual(fieldsCopy[ChannelScheme.Name], arrangeName);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValueAndCorrectType_GetTrueAndValue()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            bool isGetValue = fields.TryGetFieldValue(ChannelScheme.Name, out string name);

            // Assert
            Assert.AreEqual(isGetValue, true);
            Assert.AreEqual(name, arrangeName);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValueAndNullableType_GetTrueAndValue()
        {
            // Arrange
            var arrangeGroupId = Guid.NewGuid();
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.GroupId] = arrangeGroupId
            };

            // Act
            bool isGetValue = fields.TryGetFieldValue(ChannelScheme.GroupId, out Guid? groupId);

            // Assert
            Assert.IsTrue(isGetValue);
            Assert.IsTrue(groupId.HasValue);
            Assert.AreEqual(groupId.Value, arrangeGroupId);
        }

        [TestMethod]
        public void TryGetFieldValue_ExistingValueAndIncorrectType_GetFalse()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            bool isGetValue = fields.TryGetFieldValue(ChannelScheme.Name, out bool value);

            // Assert
            Assert.AreEqual(isGetValue, false);
        }

        [TestMethod]
        public void TryGetFieldValue_NotExistingValueCorrectType_GetFalse()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            bool isGetValue = fields.TryGetFieldValue(ChannelScheme.Description, out string value);

            // Assert
            Assert.AreEqual(isGetValue, false);
        }

        [TestMethod]
        public void TryGetFieldValue_NullFieldName_ArgumentNullException()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => fields.TryGetFieldValue(null, out string name));
        }

        [TestMethod]
        public void GetFieldValue_ExistingValueAndCorrectType_GetValue()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            var name = fields.GetFieldValue<string>(ChannelScheme.Name);

            // Assert
            Assert.AreEqual(name, arrangeName);
        }

        [TestMethod]
        public void GetFieldValue_ExistingValueAndNullableType_GetValue()
        {
            // Arrange
            var arrangeGroupId = Guid.NewGuid();
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.GroupId] = arrangeGroupId
            };

            // Act
            var groupId = fields.GetFieldValue<Guid?>(ChannelScheme.GroupId);

            // Assert
            Assert.IsTrue(groupId.HasValue);
            Assert.AreEqual(groupId.Value, arrangeGroupId);
        }

        [TestMethod]
        public void GetFieldValue_ExistingValueAndIncorrectType_Exception()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            // Assert
            Assert.ThrowsException<Exception>(() => fields.GetFieldValue<bool>(ChannelScheme.Name));
        }

        [TestMethod]
        public void GetFieldValue_NotExistingValueCorrectType_KeyNotFoundException()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            // Assert
            Assert.ThrowsException<KeyNotFoundException>(() => fields.GetFieldValue<string>(ChannelScheme.Description));
        }

        [TestMethod]
        public void GetFieldValue_NullFieldName_ArgumentNullException()
        {
            // Arrange
            var arrangeName = "name";
            var fields = new FieldValueCollection()
            {
                [ChannelScheme.Name] = arrangeName
            };

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => fields.GetFieldValue<string>(null));
        }
    }
}
