using DataManagementServer.Common.Models;
using DataManagementServer.Common.Schemes;
using DataManagementServer.Core.Services.Concrete;
using DataManagementServer.Sdk.Channels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManagementServer.Core.Tests.Services
{
    [TestClass]
    public class GroupServiceTest
    {
        [TestMethod]
        public void Create()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;

            //Act
            var groupId = groupService.Create();

            //Assert
            Assert.AreEqual(1, groupService.Count);
            Assert.AreNotEqual(Guid.Empty, groupId);
        }

        [TestMethod]
        public void CreateByParentId_Retrieve()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var parentId = groupService.Create();

            //Act
            var groupId = groupService.Create(parentId);
            var groupModel = groupService.Retrieve(groupId);

            //Assert
            Assert.AreEqual(2, groupService.Count);
            Assert.AreNotEqual(Guid.Empty, groupId);
            Assert.AreEqual(parentId, groupModel.ParentId);
        }

        [TestMethod]
        public void CreateByModel_Retrieve()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
                ParentId = Guid.NewGuid()
            };

            //Act
            var groupId = groupService.Create(groupModel);
            var groupModel2 = groupService.Retrieve(groupId);

            //Assert
            Assert.AreEqual(groupId, groupModel2.Id);
            Assert.AreEqual(groupModel.Name, groupModel2.Name);
            Assert.AreEqual(groupModel.ParentId, groupModel2.ParentId);
        }

        [TestMethod]
        public void CreateByNullModel_ArgumentNullException()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;

            //Act
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => groupService.Create(null));
        }

        [TestMethod]
        public void RetrieveAll_AllFields()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            var channelModels = groupService.RetrieveAll(true);

            //Assert
            Assert.AreEqual(4, channelModels.Count);
            channelModels.ForEach(model => 
            {
                Assert.AreEqual(2, model.Fields.Count);
                Assert.IsTrue(model.TryGetFieldValue<string>(GroupScheme.Name, out _));
                Assert.IsTrue(model.TryGetFieldValue<Guid>(GroupScheme.ParentId, out _));
            });
        }

        [TestMethod]
        public void RetrieveByLinq()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            groupModel.Name = "New group name";
            groupService.Create(groupModel);

            //Act
            var channelModels = groupService.RetrieveAll(true)
                .Where(g => string.Compare(g.Name, groupModel.Name, StringComparison.InvariantCulture) == 0)
                .ToList();

            //Assert
            Assert.AreEqual(1, channelModels.Count);
        }

        [TestMethod]
        public void RetrieveAll_NotAllFields()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            var channelModels = groupService.RetrieveAll();

            //Assert
            Assert.AreEqual(4, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(0, model.Fields.Count);
                Assert.IsFalse(model.TryGetFieldValue<string>(GroupScheme.Name, out _));
                Assert.IsFalse(model.TryGetFieldValue<Guid>(GroupScheme.ParentId, out _));
            });
        }

        [TestMethod]
        public void RetrieveByParent_AllFields()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            var parentGroupId = groupService.Create(groupModel);

            groupModel.ParentId = parentGroupId;

            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            var channelModels = groupService.RetrieveByParent(parentGroupId, true);

            //Assert
            Assert.AreEqual(3, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(2, model.Fields.Count);
                Assert.IsTrue(model.TryGetFieldValue<string>(GroupScheme.Name, out _));
                Assert.IsTrue(model.TryGetFieldValue<Guid>(GroupScheme.ParentId, out _));
            });
        }

        [TestMethod]
        public void RetrieveByParent_NotAllFields()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            var parentGroupId = groupService.Create(groupModel);

            groupModel.ParentId = parentGroupId;

            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            var channelModels = groupService.RetrieveByParent(parentGroupId);

            //Assert
            Assert.AreEqual(3, channelModels.Count);
            channelModels.ForEach(model =>
            {
                Assert.AreEqual(0, model.Fields.Count);
                Assert.IsFalse(model.TryGetFieldValue<string>(GroupScheme.Name, out _));
                Assert.IsFalse(model.TryGetFieldValue<Guid>(GroupScheme.ParentId, out _));
            });
        }

        [TestMethod]
        public void Update()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;

            var groupId = groupService.Create();
            var groupModel = new GroupModel(groupId)
            {
                Name = "Test group"
            };

            //Act
            groupService.Update(groupModel);
            var groupModel2 = groupService.Retrieve(groupId);

            //Assert
            Assert.AreEqual(groupModel.Name, groupModel2.Name);
        }

        [TestMethod]
        public void Update_ArgumentNullException()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;

            var groupId = groupService.Create();

            //Act
            //Assert
            Assert.ThrowsException<ArgumentNullException>(() => groupService.Update(null));
        }

        [TestMethod]
        public void Update_KeyNotFoundException()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;

            var groupId = groupService.Create();
            var model = new GroupModel()
            {
                Name = "Test group"
            };

            //Act
            //Assert
            Assert.ThrowsException<KeyNotFoundException>(() => groupService.Update(model));
        }

        [TestMethod]
        public void Delete_WithoutChildren()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            var parentGroupId = groupService.Create(groupModel);

            groupModel.ParentId = parentGroupId;

            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            groupService.Delete(parentGroupId);
            var groups = groupService.RetrieveAll(true);

            //Assert
            Assert.AreEqual(3, groups.Count);
            groups.ForEach(g => Assert.AreNotEqual(parentGroupId, g.ParentId));
        }

        [TestMethod]
        public void Delete_WithChildren()
        {
            //Arange
            var groupService = new OrganizationService() as IGroupService;
            var groupModel = new GroupModel()
            {
                Name = "Test group",
            };
            var parentGroupId = groupService.Create(groupModel);

            groupModel.ParentId = parentGroupId;

            groupService.Create(groupModel);
            groupService.Create(groupModel);
            groupService.Create(groupModel);

            //Act
            groupService.Delete(parentGroupId, true);

            //Assert
            Assert.AreEqual(0, groupService.Count);
        }
    }
}
