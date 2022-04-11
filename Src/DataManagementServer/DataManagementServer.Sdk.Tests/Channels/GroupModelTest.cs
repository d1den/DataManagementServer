using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Newtonsoft.Json;
using DataManagementServer.Sdk.Channels;

namespace DataManagementServer.Sdk.Tests.Channels
{
    [TestClass]
    public class GroupModelTest
    {
        [TestMethod]
        public void SetParentId_FieldsContainValue()
        {
            // Arrange
            var arrangeParentId = Guid.NewGuid();

            var model = new GroupModel()
            {
                ParentId = arrangeParentId
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(GroupScheme.ParentId, out Guid parentId);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(parentId, arrangeParentId);
        }

        [TestMethod]
        public void GetParentId_FieldsContainParentId_GetValue()
        {
            // Arrange
            var arrangeParentId = Guid.NewGuid();

            var model = new GroupModel()
            {
                [GroupScheme.ParentId] = arrangeParentId
            };

            // Act
            var parentId = model.ParentId;

            // Assert
            Assert.IsTrue(parentId.HasValue);
            Assert.AreEqual(parentId.Value, arrangeParentId);
        }

        [TestMethod]
        public void GetParentId_FieldsDontContainParentId_GetNull()
        {
            // Arrange
            var model = new GroupModel();

            // Act
            var parentId = model.ParentId;

            // Assert
            Assert.IsFalse(parentId.HasValue);
        }

         [TestMethod]
        public void SetName_FieldsContainValue()
        {
            // Arrange
            var arrangeName = "Test";

            var model = new GroupModel()
            {
                Name = arrangeName
            };

            // Act
            var isContains = model.Fields
                .TryGetFieldValue(GroupScheme.Name, out string name);

            // Assert
            Assert.IsTrue(isContains);
            Assert.AreEqual(name, arrangeName);
        }

        [TestMethod]
        public void GetName_FieldsContainName_GetValue()
        {
            // Arrange
            var arrangeName = "Test";

            var model = new GroupModel()
            {
                [GroupScheme.Name] = arrangeName
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
            var model = new GroupModel();

            // Act
            var name = model.Name;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(name));
        }

        [TestMethod]
        public void Serialize_FieldsContainName_GetCorrectJson()
        {
            // Arrange
            var arrangeName = "Test";
            var model = new GroupModel();
            model.Name = arrangeName;

            // Act
            var json = JsonConvert.SerializeObject(model);

            // Assert
            Assert.IsFalse(json.Contains(GroupScheme.ParentId));
            Assert.IsTrue(json.Contains("Fields"));
            Assert.IsTrue(json.Contains(GroupScheme.Name));
            Assert.IsTrue(json.Contains(arrangeName));
        }
    }
}
