using DataManagementServer.Common.Models;
using DataManagementServer.Core.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DataManagementServer.Core.Tests.Channels
{
    [TestClass]
    public class GroupTest
    {
        [TestMethod]
        public void Update_Name()
        {
            // Arrange
            var arrangeName = "Test name";
            var group = new Group();
            var model = new GroupModel(group.Id) { Name = arrangeName };

            // Act
            group.Update(model);
            var newModel = group.ToModel(true);

            // Assert
            Assert.AreEqual(newModel.Name, arrangeName);
        }

        [TestMethod]
        public void Update_ParentIdAndName()
        {
            // Arrange
            var arrangeParentId = Guid.NewGuid();
            var arrangeName = "Test name";
            var group = new Group();
            var model = new GroupModel(group.Id) { Name = arrangeName, ParentId = arrangeParentId };

            // Act
            group.Update(model);
            var newModel = group.ToModel(true);

            // Assert
            Assert.AreEqual(newModel.ParentId, arrangeParentId);
            Assert.AreEqual(newModel.Name, arrangeName);
        }

        [TestMethod]
        public void Update_NewId_ArgumentException()
        {
            // Arrange
            var group = new Group();
            var model = new GroupModel(Guid.NewGuid());

            // Act
            // Assert
            Assert.ThrowsException<ArgumentException>(() => group.Update(model));
        }

        [TestMethod]
        public void Update_NullModel_ArgumentNullException()
        {
            // Arrange
            var group = new Group();

            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => group.Update(null));
        }

        [TestMethod]
        public void ToModel_AllFields_GetAllFields()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeParentId = Guid.NewGuid();
            var arrangeName = "Test name";

            var model = new GroupModel(arrangeId) { ParentId = arrangeParentId, Name = arrangeName };
            var group = new Group(model);

            // Act
            var newModel = group.ToModel(true);

            // Assert
            Assert.AreEqual(newModel.Id, arrangeId);
            Assert.AreEqual(newModel.ParentId, arrangeParentId);
            Assert.AreEqual(newModel.Name, arrangeName);
        }

        [TestMethod]
        public void ToModel_NotAllFields_GetId()
        {
            // Arrange
            var arrangeId = Guid.NewGuid();
            var arrangeParentId = Guid.NewGuid();
            var arrangeName = "Test name";

            var model = new GroupModel(arrangeId) { ParentId = arrangeParentId, Name = arrangeName };
            var group = new Group(model);

            // Act
            var newModel = group.ToModel(false);

            // Assert
            Assert.AreEqual(newModel.Id, arrangeId);
            Assert.IsFalse(newModel.ParentId.HasValue);
            Assert.IsTrue(string.IsNullOrEmpty(newModel.Name));
        }

        [TestMethod]
        public void Contstructor_Empty_GetGroupWithId()
        {
            // Arrange
            // Act
            var group = new Group();
            var newModel = group.ToModel(true);
            // Assert
            Assert.AreNotEqual(group.Id, Guid.Empty);
            Assert.AreEqual(newModel.ParentId, Guid.Empty);
            Assert.IsNotNull(newModel.Name);
        }

        [TestMethod]
        public void Contstructor_ModelWithEmptyId_GetGroupWithId()
        {
            // Arrange
            var model = new GroupModel();

            // Act
            var group = new Group(model);
            var newModel = group.ToModel(true);
            // Assert
            Assert.AreNotEqual(group.Id, Guid.Empty);
            Assert.AreEqual(newModel.ParentId, Guid.Empty);
            Assert.IsNotNull(newModel.Name);
        }

        [TestMethod]
        public void Contstructor_ModelWithIdAndName_GetGroupWithIdAndName()
        {
            // Arrange
            var arrangeName = "Test";
            var model = new GroupModel(Guid.NewGuid())
            {
                Name = arrangeName
            };

            // Act
            var group = new Group(model);
            var newModel = group.ToModel(true);
            // Assert
            Assert.AreNotEqual(group.Id, Guid.Empty);
            Assert.AreEqual(newModel.ParentId, Guid.Empty);
            Assert.AreEqual(string.Compare(arrangeName, newModel.Name), 0);
        }

        [TestMethod]
        public void Contstructor_NullModel_ArgumentNullException()
        {
            // Arrange
            // Act
            // Assert
            Assert.ThrowsException<ArgumentNullException>(() => new Group(null));
        }
    }
}
